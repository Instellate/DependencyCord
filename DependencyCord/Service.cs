using DependencyCord.Internal;
using DependencyCord.Types;
using DependencyCord.Services;
using DependencyCord.Services.Cache;
using DependencyCord.Controllers;
using System.Text.Json;
using System.Reflection;
using Websocket.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DependencyCord;

public static class DependencyCordExtensionServices
{
    /// <summary>
    /// The primary (and only way) to add all the services for DependencyCord to work.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="token">Your bot token.</param>
    /// <param name="intents">The intents you want.</param>
    /// <param name="asm">Assembly to reflect all the controllers and methods.</param>
    public static void AddDependencyCord(this IServiceCollection services, string token, WebSocketIntents intents, Assembly asm)
    {
        services.AddSingleton<WebsocketClient>((service) => new WebsocketClient(
            new Uri("wss://gateway.discord.gg/?v=10&encoding=json")));
        services.AddHostedService<WsService>();
        services.AddSingleton<DCConfigs>((service) => new DCConfigs { BotToken = token, Intents = intents });
        services.AddSingleton<DependencyCordEvents>();
        services.AddSingleton<GuildCache>((services) =>
        new GuildCache(services.GetRequiredService<DCConfigs>(), services.GetRequiredService<IHttpClientFactory>()));

        services.AddSingleton<ControllerManager>((services) => new ControllerManager(asm, services));
        {
            var types = asm.GetTypes();
            var orderedTypes = types.Where(t => t.IsSubclassOf(typeof(BaseController)));

            foreach (var type in orderedTypes)
            {
                services.AddScoped(type);
            }
        }

        services.AddLogging();
        services.AddHttpClient();
        services.AddHttpClient("Discord", (httpClient) =>
        {
            httpClient.BaseAddress = new Uri("https://discord.com/api/v10/");
            httpClient.DefaultRequestHeaders.Authorization = new("bot", token);
        });
        services.Remove(services.FirstOrDefault(desc => desc.ServiceType == typeof(IHttpMessageHandlerBuilderFilter))!);
    }
}

internal class WsService : IHostedService, IDisposable
{
    // internal Timer? _wsPingTimer;
    internal WebsocketClient _ws;
    internal DCConfigs _config;
    // internal int? _lastSequenceNumber;
    internal CancellationTokenSource _cts = new CancellationTokenSource();
    internal DependencyCordEvents _events;
    internal List<string> _guildsRegistering = new List<string>();
    internal GuildCache _guildCache;
    internal ControllerManager _manager;

    public WsService(WebsocketClient webSocket, DCConfigs config, DependencyCordEvents events, GuildCache guildCache,
        ControllerManager manager)
    {
        _ws = webSocket;
        _config = config;
        _events = events;
        _guildCache = guildCache;
        _manager = manager;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _manager.RegisterControllers();
        _ws.IsReconnectionEnabled = false;
        _ws.MessageReceived.Subscribe(HandleEvents);
        await _ws.Start();
        string data = JsonSerializer.Serialize(new WebSocketBaseSend<WebSocketIdentify>
        {
            Opcode = WebSocketOpCode.Identify,
            Data = new WebSocketIdentify
            {
                Token = _config.BotToken,
                Intents = _config.Intents
            }
        }, JsonOptions.Op);
        _ws.Send(data);
    }

    public Task StopAsync(CancellationToken ct)
    {
        _cts.Cancel();
        Console.WriteLine($"It is {_ws.IsRunning} that it is connected.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }

    private async void HandleEvents(ResponseMessage msg)
    {
        if (msg.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
        {
            Console.WriteLine("Connection closed!");
        }
        var data = await JsonSerializer.DeserializeAsync<WebSocketBaseReceive>(
            new MemoryStream(System.Text.Encoding.Default.GetBytes(msg.Text)));


        if (data!.OpCode != WebSocketOpCode.Dispatch)
        {
            if (data.OpCode == WebSocketOpCode.InvalidSession)
                throw new InvalidBotTokenException("Bot token given is invalid.");
        }
        if (data.Type == "READY")
        {
            var readyData = JsonSerializer.Deserialize<ReadyData>(data.Data, JsonOptions.Op);
            _events.InvokeReadyEvent(readyData!.User);
            foreach (var guild in readyData.Guilds) _guildsRegistering.Add(guild.Id);
        }
        else if (data.Type == "GUILD_CREATE")
        {
            IGuild guild = (JsonSerializer.Deserialize<JsonGuild>(data.Data, JsonOptions.Op))!;
            if (_guildsRegistering.Count == 0)
            {
                _events.InvokeGuildCreateEvent(guild!);
            }
            else if (_guildsRegistering.Contains(guild?.Id ?? ""))
            {
                _guildsRegistering.Remove(guild!.Id);
                _guildCache._guilds.Add(guild.Id, guild);
                return;
            }
            else
            {
                if (!_guildCache._guilds.TryGetValue(guild!.Id, out var dicGuild))
                {
                    _guildCache._guilds.Add(guild.Id, guild);
                }
                _events.InvokeGuildCreateEvent(guild);
            }
        }
        else if (data.Type == "GUILD_DELETE")
        {
            var guild = JsonSerializer.Deserialize<UnavailableGuild>(data.Data, JsonOptions.Op);
            _guildCache._guilds.Remove(guild?.Id ?? "");
        }
        else if (data.Type == "INTERACTION_CREATE")
        {
            var interaction = JsonSerializer.Deserialize<InteractionObject>(data.Data, JsonOptions.Op);
            if (interaction!.Type != InteractionType.ApplicationCommand)
            {
                Console.WriteLine("Got interaction create with type " + interaction.Type);
                return;
            }
            if (interaction.GuildId is not null) interaction.Guild = await _guildCache.FetchGuild(interaction.GuildId);

            await _manager.ExecuteCommand(interaction);
        }
    }
}

internal static class TaskExtensions
{
    internal static Task<T> AsCancellable<T>(this Task<T> task, CancellationToken token)
    {
        if (!token.CanBeCanceled)
        {
            return task;
        }

        var tcs = new TaskCompletionSource<T>();
        token.Register(() => tcs.TrySetCanceled(token),
            useSynchronizationContext: false);

        task.ContinueWith(t =>
            {
                if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    tcs.TrySetException(t.Exception!);
                }
                else
                {
                    tcs.TrySetResult(t.Result);
                }
            },
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

        return tcs.Task;
    }
}
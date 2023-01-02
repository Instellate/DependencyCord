using System.Reflection;
using System.Net.Http.Json;
using DependencyCord.Types;
using DependencyCord.Internal;
using DependencyCord.Services.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyCord.Controllers;

internal record class CommandInfo(Type ClassT, MethodInfo Method, bool IsAsync);
internal class ControllerManager
{
    private IServiceProvider _services;
    private Assembly _asm;
    private Dictionary<string, CommandInfo> _commandInfos = new Dictionary<string, CommandInfo>();

    public ControllerManager(Assembly asm, IServiceProvider services)
    {
        _asm = asm;
        _services = services;
    }

    public void RegisterControllers()
    {
        var types = _asm.GetTypes();
        var orderedTypes = types
            .Where(t => t.IsClass)
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsSubclassOf(typeof(BaseController)));

        foreach (var type in orderedTypes)
        {
            if (type.GetCustomAttribute<ControllerAttribute>() is null) continue;
            foreach (var method in type.GetMethods())
            {
                var cnAttr = method.GetCustomAttribute<CommandAttribute>();
                if (cnAttr is null) continue;
                if (method.IsStatic) continue;
                bool isAsync = (method.ReturnType == typeof(Task<ControllerResult>));
                _commandInfos.Add(cnAttr.CommandName, new CommandInfo(type, method, isAsync));
            }
        }
    }

    public async Task<bool> ExecuteCommand(InteractionObject interaction)
    {
        if (_commandInfos.TryGetValue(interaction.Data.Name, out var ci))
        {
            using (var scope = _services.CreateScope())
            {
                var obj = scope.ServiceProvider.GetRequiredService(ci.ClassT) as BaseController;
                obj!.Data = interaction;
                IControllerResult result;
                if (ci.IsAsync)
                {
                    result = (await (ci.Method.Invoke(obj, null) as Task<ControllerResult>)!);
                }
                else result = (ci.Method.Invoke(obj, null) as IControllerResult)!;

                var client = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Discord");
                var interactionResponse = new InteractionResponseObject();
                interactionResponse.Type = InteractionResponseCallbackType.ChannelMessageWithSource;
                interactionResponse.Data = new InteractionResponseData();
                interactionResponse.Data.Content = result.Message;
                interactionResponse.Data.Embeds = result.Embeds;

                await client.PostAsJsonAsync($"interactions/{interaction.Id}/{interaction.Token}/callback",
                interactionResponse, JsonOptions.Op);
            }
            return true;
        }
        else return false;
    }
}

public abstract class BaseController
{
    /// <summary>
    /// Raw data provided by discord
    /// </summary>
    public InteractionObject Data { get; set; } = null!;
}

public interface IControllerResult
{
    public string? Message { get; set; }
    public List<Embed>? Embeds { get; set; }
}

public class ControllerResult : IControllerResult
{
    public static implicit operator ControllerResult(string message)
    {
        return new ControllerResult
        {
            Message = message
        };
    }

    public static implicit operator ControllerResult(Embed embed)
    {
        var result = new ControllerResult();
        result.Embeds = new List<Embed>();
        result.Embeds.Add(embed);
        return result;
    }

    public string? Message { get; set; }
    public List<Embed>? Embeds { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class CommandAttribute : Attribute
{
    public string CommandName { get; set; }

    public CommandAttribute(string name)
    {
        CommandName = name;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ControllerAttribute : Attribute
{
    public string? CommandGroupName { get; set; }

    public ControllerAttribute(string name)
    {
        CommandGroupName = name;
    }

    public ControllerAttribute()
    {

    }
}
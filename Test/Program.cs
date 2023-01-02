using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using DependencyCord.Types;
using DependencyCord.Services;
using DependencyCord;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddUserSecrets<Program>();
        builder.Services.AddDependencyCord(builder.Configuration["BotToken"]!,
        WebSocketIntents.Guilds | WebSocketIntents.GuildMembers, Assembly.GetExecutingAssembly());

        var app = builder.Build();
        var events = app.Services.GetService(typeof(DependencyCordEvents)) as DependencyCordEvents;
        events!.ReadyEvent += Ready;
        events.GuildCreateEvent += GuildCreate;

        app.Run();
    }

    private static void Ready(IUser user)
    {
        Console.WriteLine($"Ready as {user.Username}#{user.Discriminator}");
    }

    private static void GuildCreate(IGuild guild)
    {
        Console.WriteLine($"Joined guild {guild.Name}");
    }

}
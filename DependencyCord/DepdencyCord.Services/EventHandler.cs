using DependencyCord.Types;

namespace DependencyCord.Services;

public class DependencyCordEvents
{
    public delegate void ReadyDel(IUser user);
    public event ReadyDel? ReadyEvent;
    internal void InvokeReadyEvent(IUser user)
    {
        ReadyEvent?.Invoke(user);
    }

    public delegate void GuildCreateDel(IGuild guild);
    public event GuildCreateDel? GuildCreateEvent;
    internal void InvokeGuildCreateEvent(IGuild guild)
    {
        GuildCreateEvent?.Invoke(guild);
    }
}
namespace DependencyCord.Types;

public class InteractionObject
{
    public required string Id { get; set; }
    public required string ApplicationId { get; set; }
    public required InteractionType Type { get; set; }
    public required InteractionData Data { get; set; }
    public string? GuildId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public IGuild? Guild { get; set; }
    public string? ChannelId { get; set; }
    public IMember? Member { get; set; }
    public IUser? User { get; set; }
    public required string Token { get; set; }
    public int Version { get; set; } = 1;
    // public IMessage? Messsage { get; set; }
    public required string AppPermissions { get; set; }
    public string? Locale { get; set; }
    public string? GuildLocale { get; set; }
}

public class InteractionData
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public InteractionResolvedData? Resolved { get; set; }
    public List<ApplicationCommandResponseOption>? Options { get; set; }
    public string? GuildId { get; set; }
    public string? TargetId { get; set; }
}

public enum InteractionType
{
    Ping = 1,
    ApplicationCommand = 2,
    MessageComponent = 3,
    ApplicationCommandAutocomplete = 4,
    ModalSubmit = 5
}

public enum ApplicationCommandOptionType
{
    SubCommand = 1,
    SubCommandGroup = 2,
    String = 3,
    Integer = 4,
    Boolean = 5,
    User = 6,
    Channel = 7,
    Role = 8,
    Mentionable = 9,
    Number = 10,
    Attachment = 11
}

public class InteractionResponseObject
{
    public InteractionResponseCallbackType Type { get; set; }
    public InteractionResponseData? Data { get; set; }
}

public class InteractionResponseData
{
    public string? Content { get; set; }
    public List<Embed>? Embeds { get; set; } = new List<Embed>();
    // public List<AllowedMention> AllowedMentions { get; set; }
    public MessageFlags Flags { get; set; }
    // public List<Component> Components { get; set; }
    // public List<Attachment> Attachments { get; set; }
}

public enum InteractionResponseCallbackType
{
    Pong = 1,
    ChannelMessageWithSource = 4,
    DeferredChannelMessageWithSource = 5,
    DeferredUpdateMessage = 6,
    UpdateMessage = 7,
    ApplicationCommandAutocompleteResult = 8,
    Modal = 9
}

public class ApplicationCommandResponseOption
{
    public required string Name { get; set; }
    public required ApplicationCommandOptionType Type { get; set; }
    public System.Text.Json.JsonElement? Value { get; set; }
    public List<ApplicationCommandResponseOption>? Options { get; set; }
    public bool? Focused { get; set; }
}

public class InteractionResolvedData
{
    public Dictionary<string, IUser>? Users { get; set; }
    public Dictionary<string, IMember>? Members { get; set; }
    // public Dictionary<string, IRoles>? Roles { get; set; }
    public Dictionary<string, IChannel>? Channels { get; set; }
    public Dictionary<string, IMessage>? Messages { get; set; }
    // public Dictionary<string, Attachment>? Attachmemts { get; set; }
}
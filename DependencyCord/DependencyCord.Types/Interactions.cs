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
    // public IMember? Member { get; set; }
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
}

public enum InteractionType
{
    Ping = 1,
    ApplicationCommand = 2,
    MessageComponent = 3,
    ApplicationCommandAutocomplete = 4,
    ModalSubmit = 5
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
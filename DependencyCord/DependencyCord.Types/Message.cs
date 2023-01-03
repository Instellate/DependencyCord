using DependencyCord.Internal;
using System.Text.Json.Serialization;

namespace DependencyCord.Types;

[JsonConverter(typeof(JsonInterfaceConverter<IMessageCreate, JsonMessageCreate>))]
public interface IMessageCreate
{
    public string? Content { get; set; }
    public int? Nonce { get; set; }
    public bool? Tts { get; set; }
    public List<Embed>? Embeds { get; set; }
    // public AllowedMentions AllowedMentions { get; set; }
    public MessageReference? MessageReference { get; set; }
    // public List<ComponentBase> Components { get; set; }
    public List<string>? StickerIds { get; set; }

}

public class JsonMessageCreate : IMessageCreate
{
    public string? Content { get; set; }
    public int? Nonce { get; set; }
    public bool? Tts { get; set; }
    public List<Embed>? Embeds { get; set; }
    // public AllowedMentions AllowedMentions { get; set; }
    public MessageReference? MessageReference { get; set; }
    // public List<ComponentBase> Components { get; set; }
    public List<string>? StickerIds { get; set; }

}

public class Embed
{
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public DateTime? Timestamp { get; set; }
    public int Color { get; set; } = 0x808080;
    public EmbedFooter? Footer { get; set; }
    public List<EmbedField>? Fields { get; set; } = new List<EmbedField>();
}

public class EmbedFooter
{
    public required string Text { get; set; }
    public string? IconUrl { get; set; }
    public string? ProxyIconUrl { get; set; }
}

public class EmbedField
{
    public required string Name { get; set; }
    public required string Value { get; set; }
    public bool? Inline { get; set; }
}

public class MessageReference
{
    public string? MessageId { get; set; }
    public string? ChannelId { get; set; }
    public string? GuildId { get; set; }
    public bool? FailIfNotExists { get; set; }
}

public enum MessageFlags
{
    Crossposted = 1 << 0,
    IsCrosspost = 1 << 1,
    SuppressEmbeds = 1 << 2,
    SourceMessageDeleted = 1 << 3,
    Urgent = 1 << 4,
    HasThread = 1 << 5,
    Ephemeral = 1 << 6,
    FailedToMentionSomeRolesInThread = 1 << 8,
}

[JsonConverter(typeof(JsonInterfaceConverter<IMessage, JsonMessage>))]
public interface IMessage
{
    public string Id { get; set; }
    public IUser Author { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime EditedTimestamp { get; set; }
    public bool Tts { get; set; }
    public bool MentionEveryone { get; set; }
    public List<IUser> Mentions { get; set; }
    // public List<IRoles> Roles { get; set; }
    public List<IChannel>? MentionChannels { get; set; }
    // public List<Attachment> Attachments { get; set; }
    public List<Embed> Embeds { get; set; }
    public List<Reaction>? Reactions { get; set; }
    public int? Nonce { get; set; }
    public bool Pinned { get; set; }
    public string? WebhookId { get; set; }
    public MessageType Type { get; set; }
    // public MessageActivity? Activity { get; set; }
    // public ApplicationObject? Application { get; set; }
    public string? ApplicationId { get; set; }
    public MessageReference? MessageReference { get; set; }
    public MessageFlags? Flags { get; set; }
    public IMessage? ReferencedMessage { get; set; }
    public InteractionObject? Interaction { get; set; }
    public IChannel? Thread { get; set; }
    // public List<BaseComponent>? Components { get; set; }
    // public List<MessageStickerItems>? StickerItems { get; set; }
    // public List<Sticker>? Stickers { get; set; }
    public int? Position { get; set; }
}

internal class JsonMessage : IMessage
{
    public required string Id { get; set; }
    public required IUser Author { get; set; }
    public required string Content { get; set; }
    public required DateTime Timestamp { get; set; }
    public required DateTime EditedTimestamp { get; set; }
    public required bool Tts { get; set; }
    public required bool MentionEveryone { get; set; }
    public required List<IUser> Mentions { get; set; }
    // public List<IRoles> Roles { get; set; }
    public List<IChannel>? MentionChannels { get; set; }
    // public List<Attachment> Attachments { get; set; }
    public required List<Embed> Embeds { get; set; }
    public List<Reaction>? Reactions { get; set; }
    public int? Nonce { get; set; }
    public required bool Pinned { get; set; }
    public string? WebhookId { get; set; }
    public required MessageType Type { get; set; }
    // public MessageActivity? Activity { get; set; }
    // public ApplicationObject? Application { get; set; }
    public string? ApplicationId { get; set; }
    public MessageReference? MessageReference { get; set; }
    public MessageFlags? Flags { get; set; }
    public IMessage? ReferencedMessage { get; set; }
    public InteractionObject? Interaction { get; set; }
    public IChannel? Thread { get; set; }
    // public List<BaseComponent>? Components { get; set; }
    // public List<MessageStickerItems>? StickerItems { get; set; }
    // public List<Sticker>? Stickers { get; set; }
    public int? Position { get; set; }
}

public class Reaction
{
    public required int Count { get; set; }
    public required bool Me { get; set; }
    public required Emoji Emoji { get; set; }
}

public enum MessageType
{
    Default = 0,
    RecipientAdd = 1,
    RecipientRemove = 2,
    Call = 3,
    ChannelNameChange = 4,
    ChannelIconChange = 5,
    ChannelPinnedMessage = 6,
    UserJoin = 7,
    GuildBoost = 8,
    GuildBoostTier1 = 9,
    GuildBoostTier2 = 10,
    GuildBoostTier3 = 11,
    ChannelFollowAdd = 12,
    GuilDiscoveryDisqualified = 14,
    GuilDiscoveryResqualified = 15,
    GuildDiscoveryGracePeriodInitialWarning = 16,
    GuildDiscoveryGracePeriodFinalWarning = 17,
    ThreadCreated = 18,
    Reply = 19,
    ChatInputCommand = 20,
    ThreadStarterMessage = 21,
    GuildInviteReminder = 22,
    ContextMenuCommand = 23,
    AutoModerationAction = 24
}
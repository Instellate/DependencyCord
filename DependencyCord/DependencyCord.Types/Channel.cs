using System.Text.Json.Serialization;

namespace DependencyCord.Types;

[JsonDerivedType(typeof(JsonChannel))]
public interface IChannel
{
    public string Id { get; set; }
    public ChannelTypes Type { get; set; }
    public string? GuildId { get; set; }
    [JsonIgnore]
    public IGuild? Guild { get; set; }
    public int? Position { get; set; }
    public List<PermissionOverwrite>? PermissionOverwrites { get; set; }
    public string? Name { get; set; }
    public string? Topic { get; set; }
    public bool? Nsfw { get; set; }
    public string? LastMessageId { get; set; }
    public int? Bitrate { get; set; }
    public int? UserLimit { get; set; }
    public int? RateLimitPerUser { get; set; }
    public List<IUser>? Recipients { get; set; }
    public string? Icon { get; set; }
    public string? OwnerId { get; set; }
    public string? ApplicationId { get; set; }
    public string? ParentId { get; set; }
    public DateTime? LastPinTimestamp { get; set; }
    public string? RtcRegion { get; set; }
    public int? VideoQualityMode { get; set; }
    public int? MessageCount { get; set; }
    public int? MemberCount { get; set; }
    // public ThreadMetadata ThreadMetdata { get; set; }
    // public ThreadMember Member { get; set; }
    public int? DefaultAutoArchiveDuration { get; set; }
    public string? Permissions { get; set; }
    public int? Flags { get; set; }
    public int? TotalMessageSent { get; set; }
    public List<ForumTag>? AvailableTags { get; set; }
    public List<string>? AppliedTags { get; set; }
    public DefaultReaction? DefaultReactionEmoji { get; set; }
    public int? DefaultThreadRateLimitPerUser { get; set; }
    public int? DefaultSortOrder { get; set; }
    public int? DefaultForumLayout { get; set; }
}

internal class JsonChannel : IChannel
{
    public required string Id { get; set; }
    public ChannelTypes Type { get; set; }
    public string? GuildId { get; set; }
    [JsonIgnore]
    public IGuild? Guild { get; set; }
    public int? Position { get; set; }
    public List<PermissionOverwrite>? PermissionOverwrites { get; set; }
    public string? Name { get; set; }
    public string? Topic { get; set; }
    public bool? Nsfw { get; set; }
    public string? LastMessageId { get; set; }
    public int? Bitrate { get; set; }
    public int? UserLimit { get; set; }
    public int? RateLimitPerUser { get; set; }
    public List<IUser>? Recipients { get; set; }
    public string? Icon { get; set; }
    public string? OwnerId { get; set; }
    public string? ApplicationId { get; set; }
    public string? ParentId { get; set; }
    public DateTime? LastPinTimestamp { get; set; }
    public string? RtcRegion { get; set; }
    public int? VideoQualityMode { get; set; }
    public int? MessageCount { get; set; }
    public int? MemberCount { get; set; }
    // public ThreadMetadata ThreadMetdata { get; set; }
    // public ThreadMember Member { get; set; }
    public int? DefaultAutoArchiveDuration { get; set; }
    public string? Permissions { get; set; }
    public int? Flags { get; set; }
    public int? TotalMessageSent { get; set; }
    public List<ForumTag>? AvailableTags { get; set; }
    public List<string>? AppliedTags { get; set; }
    public DefaultReaction? DefaultReactionEmoji { get; set; }
    public int? DefaultThreadRateLimitPerUser { get; set; }
    public int? DefaultSortOrder { get; set; }
    public int? DefaultForumLayout { get; set; }
}

public enum ChannelTypes
{
    GuildText = 0,
    DM = 1,
    GuildVoice = 2,
    GroupDM = 3,
    GuildCategory = 4,
    GuildAnnouncement = 5,
    AnnouncementThread = 10,
    PublicThread = 11,
    PrivateThread = 12,
    GuildStageVoice = 13,
    GuildDirectory = 14,
    GuildForum = 15
}

public class PermissionOverwrite
{
    public required string Id { get; set; }
    public PermissionOverwriteType Type { get; set; }
    [JsonIgnore]
    public IUser? User { get; set; }
    public IChannel? Channel { get; set; }
    public required string Allow { get; set; }
    public required string Deny { get; set; }
}

public enum PermissionOverwriteType
{
    Role = 0,
    Member = 1,
}

public class DefaultReaction
{
    public string? EmojiId { get; set; }
    public string? EmojiName { get; set; }
}

public class ForumTag
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required bool Moderated { get; set; }
    public required string EmojiId { get; set; }
    public string? EomjiName { get; set; }
}
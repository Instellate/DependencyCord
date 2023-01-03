using DependencyCord.Internal;
using System.Text.Json.Serialization;

namespace DependencyCord.Types;

[JsonConverter(typeof(JsonInterfaceConverter<IGuild, JsonGuild>))]
public interface IGuild
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? IconHash { get; set; }
    public string? Splash { get; set; }
    public string? DiscoverySplash { get; set; }
    public bool? Owner { get; set; }
    public string OwnerId { get; set; }
    public string? Permissions { get; set; }
    public string? Region { get; set; }
    public string? AfkChannelId { get; set; }
    public int? AfkTimeout { get; set; }
    public bool? WidgetEnabled { get; set; }
    public string? WidgetChannelId { get; set; }
    public int VerificationLevel { get; set; }
    public int DefaultMessageNotifications { get; set; }
    public int ExplicitContentFilter { get; set; }
    // public List<Roles> Roles { get; set; }
    public List<Emoji> Emojis { get; set; }
    public List<string> Features { get; set; }
    public int MfaLevel { get; set; }
    public string? ApplicationId { get; set; }
    public string? SystemChannelId { get; set; }
    public int SystemChannelFlags { get; set; }
    public string? RulesChannelId { get; set; }
    public int? MaxPresences { get; set; }
    public int? MaxMembers { get; set; }
    public string? VanityUrlCode { get; set; }
    public string? Description { get; set; }
    public string? Banner { get; set; }
    public int? PremiumTier { get; set; }
    public int? PremiumSubscriptionCount { get; set; }
    public string PreferredLocale { get; set; }
    public string? PublicUpdatesChannelId { get; set; }
    public int? MaxVideoChannelUsers { get; set; }
    public int? ApproximatePresenceCount { get; set; }
    // public WelcomeScreen WelcomeScreen { get; set; }
    public int NsfwLevel { get; set; }
    // public List<Sticker> Stickers { get; set; }
    public bool PremiumProgressBarEnabled { get; set; }
}

internal class JsonGuild : IGuild
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string? Icon { get; set; }
    public string? IconHash { get; set; }
    public string? Splash { get; set; }
    public string? DiscoverySplash { get; set; }
    public bool? Owner { get; set; }
    public required string OwnerId { get; set; }
    public string? Permissions { get; set; }
    public string? Region { get; set; }
    public string? AfkChannelId { get; set; }
    public int? AfkTimeout { get; set; }
    public bool? WidgetEnabled { get; set; }
    public string? WidgetChannelId { get; set; }
    public required int VerificationLevel { get; set; }
    public required int DefaultMessageNotifications { get; set; }
    public required int ExplicitContentFilter { get; set; }
    // public List<Roles> Roles { get; set; }
    public required List<Emoji> Emojis { get; set; }
    public required List<string> Features { get; set; }
    public required int MfaLevel { get; set; }
    public string? ApplicationId { get; set; }
    public string? SystemChannelId { get; set; }
    public required int SystemChannelFlags { get; set; }
    public string? RulesChannelId { get; set; }
    public int? MaxPresences { get; set; }
    public int? MaxMembers { get; set; }
    public string? VanityUrlCode { get; set; }
    public string? Description { get; set; }
    public string? Banner { get; set; }
    public int? PremiumTier { get; set; }
    public int? PremiumSubscriptionCount { get; set; }
    public required string PreferredLocale { get; set; }
    public string? PublicUpdatesChannelId { get; set; }
    public int? MaxVideoChannelUsers { get; set; }
    public int? ApproximatePresenceCount { get; set; }
    // public WelcomeScreen WelcomeScreen { get; set; }
    public required int NsfwLevel { get; set; }
    // public List<Sticker> Stickers { get; set; }
    public required bool PremiumProgressBarEnabled { get; set; }
}

public class Emoji
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    // public List<IRoles>? Roles { get; set; }
    public List<IUser>? Users { get; set; }
    public bool? RequireColons { get; set; }
    public bool? Managed { get; set; }
    public bool? Animated { get; set; }
    public bool? Available { get; set; }
}

[JsonConverter(typeof(JsonInterfaceConverter<IMember, JsonMember>))]
public interface IMember
{
    public IUser? User { get; set; }
    [JsonPropertyName("nick")]
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
    // public List<IRole> Roles { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? PremiumSince { get; set; }
    public bool Deaf { get; set; }
    public bool Mute { get; set; }
    public bool? Pending { get; set; }
    public string? Permissions { get; set; }
    public DateTime? CommunicationDisabledUntil { get; set; }
}

internal class JsonMember : IMember
{
    public IUser? User { get; set; }
    [JsonPropertyName("nick")]
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
    // public List<IRole> Roles { get; set; }
    public required DateTime JoinedAt { get; set; }
    public DateTime? PremiumSince { get; set; }
    public bool Deaf { get; set; }
    public bool Mute { get; set; }
    public bool? Pending { get; set; }
    public string? Permissions { get; set; }
    public DateTime? CommunicationDisabledUntil { get; set; }
}
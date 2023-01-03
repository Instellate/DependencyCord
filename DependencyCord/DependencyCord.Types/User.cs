using DependencyCord.Internal;
using System.Text.Json.Serialization;

namespace DependencyCord.Types;

[JsonConverter(typeof(JsonInterfaceConverter<IUser, JsonUser>))]
public interface IUser
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Discriminator { get; set; }
    public string Avatar { get; set; }
    public bool? Bot { get; set; }
    public bool? System { get; set; }
    public bool? MfaEnabled { get; set; }
    public string? Banner { get; set; }
    public int? AccentColor { get; set; }
    public string? Locale { get; set; }
    public bool? Verified { get; set; }
    public string? Email { get; set; }
    public int? Flags { get; set; }
    public int? PremiumType { get; set; }
    public int? PublicFlags { get; set; }
}

internal class JsonUser : IUser
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Discriminator { get; set; }
    public required string Avatar { get; set; }
    public bool? Bot { get; set; }
    public bool? System { get; set; }
    public bool? MfaEnabled { get; set; }
    public string? Banner { get; set; }
    public int? AccentColor { get; set; }
    public string? Locale { get; set; }
    public bool? Verified { get; set; }
    public string? Email { get; set; }
    public int? Flags { get; set; }
    public int? PremiumType { get; set; }
    public int? PublicFlags { get; set; }
}
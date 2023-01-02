using System.Text.Json.Serialization;
using System.Text.Json;

namespace DependencyCord.Types;

internal class WebSocketBaseSend<T>
{
    [JsonPropertyName("op")]
    public required WebSocketOpCode Opcode { get; set; }

    [JsonPropertyName("d")]
    public required T Data { get; set; }

    [JsonPropertyName("s")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? SequenceNumber { get; set; }
}

internal class WebSocketBaseReceive
{
    [JsonPropertyName("op")]
    public required WebSocketOpCode OpCode { get; set; }

    [JsonPropertyName("t")]
    public string? Type { get; set; }

    [JsonPropertyName("d")]
    public required JsonDocument Data { get; set; }
}

internal class WebSocketIdentify
{
    public required string Token { get; set; }
    public required WebSocketIntents Intents { get; set; }
    public WebSocketIdentifyProp Properties { get; set; } = new WebSocketIdentifyProp();
}

internal class WebSocketIdentifyProp
{
    public string Os { get; set; } = "linux";
    public string Browser { get; set; } = "DependencyCord";
    public string Device { get; set; } = "DependencyCord";
}

internal enum WebSocketOpCode
{
    Dispatch = 0,
    Heartbeat = 1,
    Identify = 2,
    PresenceUpdate = 3,
    VoiceStateUpdate = 4,
    Resume = 6,
    Reconnect = 7,
    RequestGuildMembers = 8,
    InvalidSession = 9,
    Hello = 10,
    HeartbeatACK = 11,
}

public enum WebSocketIntents : ulong
{
    Guilds = 1 << 0,
    GuildMembers = 1 << 1,
    GuildBans = 1 << 2,
    GuildEmojisAndStickers = 1 << 3,
    GuildIntegrations = 1 << 4,
    GuildWebhooks = 1 << 5,
    GuildInvites = 1 << 6,
    GuildVoiceStates = 1 << 7,
    GuildPresenses = 1 << 8,
    GuildMessages = 1 << 9,
    GuildMessageReactions = 1 << 10,
    GuildMessageTyping = 1 << 11,
    DirectMessages = 1 << 12,
    DirectMessageReactions = 1 << 13,
    DirectMessageTyping = 1 << 14,
    MessageContent = 1 << 15,
    GuildScheduledEvents = 1 << 16,
    AutoModerationConfiguration = 1 << 20,
    AutoModerationExecution = 1 << 21,
}

internal class ReadyData
{
    [JsonPropertyName("v")]
    public required int Version { get; set; }
    public required JsonUser User { get; set; }
    public required List<UnavailableGuild> Guilds { get; set; }
    public required string SessionId { get; set; }
    public required string ResumeGatewayUrl { get; set; }
}

internal class UnavailableGuild
{
    public required string Id { get; set; }
    public required bool Unavailable { get; set; }
}
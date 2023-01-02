using DependencyCord.Types;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DependencyCord.Internal;

internal class DCConfigs
{
    public required string BotToken { get; set; }
    public required WebSocketIntents Intents { get; set; }
}

internal class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

    public override string ConvertName(string str)
    {
        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}

internal static class JsonOptions
{
    public static JsonSerializerOptions Op { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DependencyCord.Internal;

/// <summary>
/// A class that can help with fallbacking when converting interfaces.
/// </summary>
/// <typeparam name="TType">The interface</typeparam>
/// <typeparam name="TImplementation">The type to fallback to.</typeparam>
public class JsonInterfaceConverter<TType, TImplementation> : JsonConverter<TType>
  where TImplementation : TType
{
  public override TType? Read(
    ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
      JsonSerializer.Deserialize<TImplementation>(ref reader, options);

  public override void Write(
    Utf8JsonWriter writer, TType value, JsonSerializerOptions options) =>
      JsonSerializer.Serialize(writer, (TImplementation)value!, options);
}
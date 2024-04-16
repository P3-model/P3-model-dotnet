using System.Text.Json;
using System.Text.Json.Serialization;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal abstract class ModelJsonConverter<T> : JsonConverter<T>
    where T : class
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteRequiredAttributes(writer, value);
        using var document = JsonSerializer.SerializeToDocument(value, value.GetType(), options);
        var additionalAttributes = GetAdditionalAttributes(document);
        if (additionalAttributes.Count > 0)
        {
            writer.WritePropertyName("AdditionalAttributes");
            writer.WriteStartObject();
            foreach (var additionalAttribute in additionalAttributes)
                additionalAttribute.WriteTo(writer);
            writer.WriteEndObject();
        }
        writer.WriteEndObject();
    }

    private IReadOnlyCollection<JsonProperty> GetAdditionalAttributes(JsonDocument document)
    {
        var properties = new List<JsonProperty>();
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator - enumerator boxing
        foreach (var property in document.RootElement.EnumerateObject())
        {
            if (IsAdditionalAttribute(property))
                properties.Add(property);
        }
        return properties;
    }


    protected abstract void WriteRequiredAttributes(Utf8JsonWriter writer, T value);
    protected abstract bool IsAdditionalAttribute(JsonProperty jsonProperty);
}
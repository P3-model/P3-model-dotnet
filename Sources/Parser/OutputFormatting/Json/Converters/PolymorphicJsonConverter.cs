using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class PolymorphicJsonConverter<T> : JsonConverter<T>
    where T : class
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("$type", value.GetType().Name);
        using var document = JsonSerializer.SerializeToDocument(value, value.GetType(), options);
        foreach (var property in document.RootElement.EnumerateObject())
            property.WriteTo(writer);
        writer.WriteEndObject();
    }
}
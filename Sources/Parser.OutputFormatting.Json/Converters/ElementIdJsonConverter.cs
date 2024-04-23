using System.Text.Json;
using System.Text.Json.Serialization;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

public class ElementIdJsonConverter : JsonConverter<ElementId>
{
    public override ElementId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null)
            throw new InvalidOperationException();
        return new ElementId(value);
    }

    public override void Write(Utf8JsonWriter writer, ElementId id, JsonSerializerOptions options) =>
        writer.WriteStringValue(id.Value);
}
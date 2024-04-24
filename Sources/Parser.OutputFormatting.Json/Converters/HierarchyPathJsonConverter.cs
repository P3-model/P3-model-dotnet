using System.Text.Json;
using System.Text.Json.Serialization;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

public class HierarchyPathJsonConverter : JsonConverter<HierarchyPath>
{
    public override HierarchyPath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null)
            throw new InvalidOperationException();
        return HierarchyPath.FromValue(value);
    }

    public override void Write(Utf8JsonWriter writer, HierarchyPath value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Full);
}
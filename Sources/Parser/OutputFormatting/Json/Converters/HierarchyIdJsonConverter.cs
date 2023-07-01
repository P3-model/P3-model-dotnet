using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

public class HierarchyIdJsonConverter : JsonConverter<HierarchyId>
{
    public override HierarchyId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null)
            throw new InvalidOperationException();
        return HierarchyId.FromValue(value);
    }

    public override void Write(Utf8JsonWriter writer, HierarchyId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.FullName);
}
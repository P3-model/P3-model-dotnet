using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

public class FileInfoJsonConverter : JsonConverter<FileInfo>
{
    public override FileInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null)
            throw new InvalidOperationException();
        return new FileInfo(value);
    }

    public override void Write(Utf8JsonWriter writer, FileInfo value, JsonSerializerOptions options)
    {
        using var streamReader = value.OpenText();
        writer.WriteStartObject();
        writer.WriteString("FileType", value.Extension.StartsWith('.') ? value.Extension[1..] : value.Extension);
        writer.WriteString("Content", streamReader.ReadToEnd());
        writer.WriteEndObject();
    }
}
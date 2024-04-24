using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting.Json.Converters;

namespace P3Model.Parser.OutputFormatting.Json;

public static class P3ModelSerializer
{
    private static readonly JsonConverter[] Converters =
    {
        new ElementIdJsonConverter(),
        new HierarchyPathJsonConverter(),
        new FileInfoJsonConverter(),
    };

    [PublicAPI]
    public static string Serialize(Model model) => JsonSerializer.Serialize(model, CreateOptions());

    [PublicAPI]
    public static async Task Serialize(Stream stream, Model model) =>
        await JsonSerializer.SerializeAsync(stream, model, CreateOptions());

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        foreach (var converter in Converters)
            options.Converters.Add(converter);
        return options;
    }
}
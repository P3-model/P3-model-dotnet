using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting.Json.Converters;

namespace P3Model.Parser.OutputFormatting.Json;

public static class P3ModelSerializer
{
    public static string Serialize(Model model)
    {
        var options = CreateOptions();
        return JsonSerializer.Serialize(model, options);
    }

    public static async Task Serialize(Stream stream, Model model)
    {
        var options = CreateOptions();
        await JsonSerializer.SerializeAsync(stream, model, options);
    }

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = new StaticReferenceHandler(),
            WriteIndented = true
        };
        options.Converters.Add(new PolymorphicJsonConverter<Element>());
        options.Converters.Add(new PolymorphicJsonConverter<Relation>());
        options.Converters.Add(new PolymorphicJsonConverter<Trait>());
        options.Converters.Add(new FileInfoJsonConverter());
        options.Converters.Add(new HierarchyIdJsonConverter());
        return options;
    }
}
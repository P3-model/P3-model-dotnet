using System.IO;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json;

public class JsonFormatter : OutputFormatter
{
    private readonly string _outputPath;

    public JsonFormatter(string outputPath) => _outputPath = outputPath;

    public async Task Write(Model model)
    {
        var directory = Path.GetDirectoryName(_outputPath);
        if (directory != null)
            Directory.CreateDirectory(directory);
        if (File.Exists(_outputPath))
            File.Delete(_outputPath);
        var fileStream = File.Create(_outputPath);
        await P3ModelSerializer.Serialize(fileStream, model);
    }
}
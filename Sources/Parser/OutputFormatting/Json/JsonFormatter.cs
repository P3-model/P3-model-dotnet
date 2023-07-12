using System.IO;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json;

public class JsonFormatter : OutputFormatter
{
    private readonly string _fileFullName;

    public JsonFormatter(string fileFullName) => _fileFullName = fileFullName;

    public Task Clean()
    {
        if (File.Exists(_fileFullName))
            File.Delete(_fileFullName);
        return Task.CompletedTask;
    }

    public async Task Write(Model model)
    {
        var directory = Path.GetDirectoryName(_fileFullName);
        if (directory != null)
            Directory.CreateDirectory(directory);
        var fileStream = File.Create(_fileFullName);
        await P3ModelSerializer.Serialize(fileStream, model);
    }
}
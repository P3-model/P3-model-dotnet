using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json;

public class JsonFormatter(string fileFullName) : OutputFormatter
{
    public Task Clean()
    {
        if (File.Exists(fileFullName))
            File.Delete(fileFullName);
        return Task.CompletedTask;
    }

    public async Task Write(Model model)
    {
        var directory = Path.GetDirectoryName(fileFullName);
        if (directory != null)
            Directory.CreateDirectory(directory);
        await using var fileStream = File.Create(fileFullName);
        await P3ModelSerializer.Serialize(fileStream, model);
        await fileStream.FlushAsync();
    }
}
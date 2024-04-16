using P3Model.Parser.ModelSyntax;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MermaidFormatter(string basePath, IEnumerable<MermaidPageFactory> pageFactories) : OutputFormatter
{
    public Task Clean()
    {
        if (Directory.Exists(basePath))
            Directory.Delete(basePath, true);
        return Task.CompletedTask;
    }

    async Task OutputFormatter.Write(Model model)
    {
        Directory.CreateDirectory(basePath);
        var modelGraph = ModelGraph.From(model);
        var pages = pageFactories
            .SelectMany(f => f.Create(basePath, modelGraph))
            .ToList()
            .AsReadOnly();
        foreach (var page in pages)
        {
            page.LinkWith(pages);
            await page.WriteToFile();
        }
    }
}
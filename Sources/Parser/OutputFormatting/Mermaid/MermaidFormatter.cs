using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MermaidFormatter : OutputFormatter
{
    private readonly string _basePath;
    private readonly IEnumerable<MermaidPageFactory> _pageFactories;

    public MermaidFormatter(string basePath, IEnumerable<MermaidPageFactory> pageFactories)
    {
        _basePath = basePath;
        _pageFactories = pageFactories;
    }

    public Task Clean()
    {
        if (Directory.Exists(_basePath))
            Directory.Delete(_basePath, true);
        return Task.CompletedTask;
    }

    async Task OutputFormatter.Write(Model model)
    {
        Directory.CreateDirectory(_basePath);
        var modelGraph = ModelGraph.From(model);
        var pages = _pageFactories
            .SelectMany(f => f.Create(_basePath, modelGraph))
            .ToList()
            .AsReadOnly();
        foreach (var page in pages)
        {
            page.LinkWith(pages);
            await page.WriteToFile();
        }
    }
}
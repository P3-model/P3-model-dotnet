using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    async Task OutputFormatter.Write(Model model)
    {
        var pages = _pageFactories
            .SelectMany(f => f.Create(_basePath, model))
            .ToList()
            .AsReadOnly();
        foreach (var page in pages)
        {
            page.LinkWith(pages);
            await page.WriteToFile();
        }
    }
}
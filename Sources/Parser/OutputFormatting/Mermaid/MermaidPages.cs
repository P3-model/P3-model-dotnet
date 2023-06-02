using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MermaidPages : OutputFormatter
{
    private readonly string _basePath;
    private readonly Dictionary<Type, MermaidPage> _pages;

    public MermaidPages(string basePath, IEnumerable<MermaidPage> pages)
    {
        _basePath = basePath;
        _pages = pages.ToDictionary(f => f.GetType());
    }

    public bool TryGetFileRelativePathFor<TFormatter>([NotNullWhen(true)] out string? path)
        where TFormatter : MermaidPage
    {
        if (_pages.TryGetValue(typeof(TFormatter), out var page))
        {
            path = page.RelativeFilePath;
            return true;
        }
        path = null;
        return false;
    }

    async Task OutputFormatter.Write(Model model)
    {
        foreach (var page in _pages.Values)
        {
            await using var markdownWriter = new MermaidWriter(_basePath, page.RelativeFilePath);
            page.Write(model, markdownWriter, this);
        }
    }
}
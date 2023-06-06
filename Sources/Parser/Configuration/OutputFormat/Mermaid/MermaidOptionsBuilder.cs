using System;
using System.Collections.Generic;
using P3Model.Parser.OutputFormatting.Mermaid;
using P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

namespace P3Model.Parser.Configuration.OutputFormat.Mermaid;

public class MermaidOptionsBuilder : MermaidOptionsBuilder.DirectoryStep, MermaidOptionsBuilder.PagesStep
{
    private readonly List<MermaidPageFactory> _pageFactories = new();
    private string? _directoryPath;

    PagesStep DirectoryStep.Directory(string path)
    {
        _directoryPath = path;
        return this;
    }

    MermaidOptionsBuilder PagesStep.UseDefaultPages()
    {
        _pageFactories.Add(new DomainModulesPageFactory());
        _pageFactories.Add(new DomainBuildingBlocksPageFactory());
        _pageFactories.Add(new DomainGlossaryPageFactory());
        return this;
    }

    public MermaidFormatter Build()
    {
        if (_directoryPath is null)
            throw new InvalidOperationException();
        return new MermaidFormatter(_directoryPath, _pageFactories.AsReadOnly());
    }

    public interface DirectoryStep
    {
        PagesStep Directory(string path);
    }

    public interface PagesStep
    {
        MermaidOptionsBuilder UseDefaultPages();
    }
}
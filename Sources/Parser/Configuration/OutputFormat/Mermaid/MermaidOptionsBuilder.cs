using System;
using System.Collections.Generic;
using P3Model.Parser.OutputFormatting.Mermaid;
using P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

namespace P3Model.Parser.Configuration.OutputFormat.Mermaid;

public class MermaidOptionsBuilder : MermaidOptionsBuilder.DirectoryStep, MermaidOptionsBuilder.PagesStep
{
    private readonly List<MermaidPage> _formatters = new();
    private string? _directoryPath;

    PagesStep DirectoryStep.Directory(string path)
    {
        _directoryPath = path;
        return this;
    }

    MermaidOptionsBuilder PagesStep.UseDefaultPages()
    {
        _formatters.Add(new ModelBoundaryMermaidPage());
        _formatters.Add(new DomainModulesMermaidPage());
        _formatters.Add(new DomainBuildingBlocksMermaidPage());
        _formatters.Add(new DomainGlossaryMermaidPage());
        return this;
    }

    public MermaidPages Build()
    {
        if (_directoryPath is null)
            throw new InvalidOperationException();
        return new MermaidPages(_directoryPath, _formatters.AsReadOnly());
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
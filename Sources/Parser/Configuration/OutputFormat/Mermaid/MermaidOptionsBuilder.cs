using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.OutputFormatting.Mermaid;

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
        var factories = typeof(MermaidPageFactory).Assembly
            .GetTypes()
            .Where(t => typeof(MermaidPageFactory).IsAssignableFrom(t))
            .Where(t =>
            {
                var constructors = t.GetConstructors();
                return constructors.Length == 1 && constructors[0].GetParameters().Length == 0;
            })
            .Select(Activator.CreateInstance)
            .Cast<MermaidPageFactory>();
        _pageFactories.AddRange(factories);
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
        [PublicAPI]
        PagesStep Directory(string path);
    }

    public interface PagesStep
    {
        [PublicAPI]
        MermaidOptionsBuilder UseDefaultPages();
    }
}
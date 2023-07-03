using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.Configuration.OutputFormat.Json;
using P3Model.Parser.Configuration.OutputFormat.Mermaid;
using P3Model.Parser.OutputFormatting;

namespace P3Model.Parser.Configuration.OutputFormat;

public class OutputFormatBuilder
{
    private readonly List<OutputFormatter> _formatters = new();

    [PublicAPI]
    public OutputFormatBuilder UseMermaid(Func<MermaidOptionsBuilder.DirectoryStep, MermaidOptionsBuilder>? configure = null)
    {
        var mermaidOptionsBuilder = new MermaidOptionsBuilder();
        configure?.Invoke(mermaidOptionsBuilder);
        var mermaidFormatter = mermaidOptionsBuilder.Build();
        _formatters.Add(mermaidFormatter);
        return this;
    }
    
    [PublicAPI]
    public OutputFormatBuilder UseJson(Func<JsonOptionsBuilder.FileStep, JsonOptionsBuilder>? configure = null)
    {
        var jsonOptionsBuilder = new JsonOptionsBuilder();
        configure?.Invoke(jsonOptionsBuilder);
        var jsonFormatter = jsonOptionsBuilder.Build();
        _formatters.Add(jsonFormatter);
        return this;
    }

    [PublicAPI]
    public OutputFormatBuilder Custom(params OutputFormatter[] formatters)
    {
        _formatters.AddRange(formatters);
        return this;
    }

    public IReadOnlyCollection<OutputFormatter> Build() => _formatters.AsReadOnly();
}
using JetBrains.Annotations;
using P3Model.Parser.OutputFormatting.Configuration;

namespace P3Model.Parser.OutputFormatting.Mermaid.Configuration;

public static class OutputFormatBuilderExtensions
{
    [PublicAPI]
    public static OutputFormatBuilder UseMermaid(this OutputFormatBuilder outputFormatBuilder,
        Func<MermaidOptionsBuilder.DirectoryStep, MermaidOptionsBuilder>? configure = null)
    {
        var mermaidOptionsBuilder = new MermaidOptionsBuilder();
        configure?.Invoke(mermaidOptionsBuilder);
        var mermaidFormatter = mermaidOptionsBuilder.Build();
        outputFormatBuilder.Use(mermaidFormatter);
        return outputFormatBuilder;
    }
}
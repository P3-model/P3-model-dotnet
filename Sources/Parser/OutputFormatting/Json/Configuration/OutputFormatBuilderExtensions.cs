using System;
using JetBrains.Annotations;
using P3Model.Parser.OutputFormatting.Configuration;

namespace P3Model.Parser.OutputFormatting.Json.Configuration;

public static class OutputFormatBuilderExtensions
{
    [PublicAPI]
    public static OutputFormatBuilder UseJson(this OutputFormatBuilder outputFormatBuilder, 
        Func<JsonOptionsBuilder.FileStep, JsonOptionsBuilder>? configure = null)
    {
        var jsonOptionsBuilder = new JsonOptionsBuilder();
        configure?.Invoke(jsonOptionsBuilder);
        var jsonFormatter = jsonOptionsBuilder.Build();
        outputFormatBuilder.Use(jsonFormatter);
        return outputFormatBuilder;
    }
}
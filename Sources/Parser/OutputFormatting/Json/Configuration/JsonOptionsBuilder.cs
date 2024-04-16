using System;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Json.Configuration;

public class JsonOptionsBuilder : JsonOptionsBuilder.FileStep {
    private string? _fileFullName;

    JsonOptionsBuilder FileStep.File(string fullName)
    {
        _fileFullName = fullName;
        return this;
    }

    public JsonFormatter Build()
    {
        if (_fileFullName is null)
            throw new InvalidOperationException();
        return new JsonFormatter(_fileFullName);
    }

    public interface FileStep
    {
        [PublicAPI]
        JsonOptionsBuilder File(string fullName);
    }
}
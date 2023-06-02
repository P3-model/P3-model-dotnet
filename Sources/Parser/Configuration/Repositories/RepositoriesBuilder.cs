using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis;

namespace P3Model.Parser.Configuration.Repositories;

public class RepositoriesBuilder
{
    private readonly List<RepositoryToAnalyze> _repositories = new();

    [PublicAPI]
    public RepositoriesBuilder Use(string directoryPath)
    {
        _repositories.Add(new RepositoryToAnalyze(directoryPath, Array.Empty<string>()));
        return this;
    }

    [PublicAPI]
    public RepositoriesBuilder Use(string directoryPath, params string[] slnPaths)
    {
        _repositories.Add(new RepositoryToAnalyze(directoryPath, slnPaths));
        return this;
    }

    public IReadOnlyList<RepositoryToAnalyze> Build() => _repositories.AsReadOnly();
}
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis;

namespace P3Model.Parser.Configuration.Repositories;

public class RepositoriesBuilder
{
    private readonly List<RepositoryToAnalyze> _repositories = new();

    [PublicAPI]
    public RepositoriesBuilder Use(string directoryPath, Func<RepositoryBuilder, RepositoryBuilder>? configure = null)
    {
        var repositoryBuilder = new RepositoryBuilder(directoryPath);
        if (configure != null)
            repositoryBuilder = configure(repositoryBuilder);
        var repository = repositoryBuilder.Build();
        _repositories.Add(repository);
        return this;
    }

    public IReadOnlyList<RepositoryToAnalyze> Build() => _repositories.AsReadOnly();
}
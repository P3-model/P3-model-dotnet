using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis;

namespace P3Model.Parser.Configuration.Repositories;

public class RepositoriesBuilder
{
    private readonly List<RepositoryBuilder> _repositoryBuilders = new();

    [PublicAPI]
    public RepositoriesBuilder Use(string directoryPath, Func<RepositoryBuilder, RepositoryBuilder>? configure = null)
    {
        var repositoryBuilder = new RepositoryBuilder(directoryPath);
        if (configure != null)
            repositoryBuilder = configure(repositoryBuilder);
        _repositoryBuilders.Add(repositoryBuilder);
        return this;
    }

    public async Task<IReadOnlyList<RepositoryToAnalyze>> Build() => await Task
        .WhenAll(_repositoryBuilders
            .Select(builder => builder.Build()));
}
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.CodeAnalysis;

namespace P3Model.Parser.Configuration.Repositories;

public class RepositoryBuilder
{
    private readonly string _path;
    private readonly List<string> _slnPaths = new();
    private readonly List<string> _excludedProjects = new();

    public RepositoryBuilder(string path) => _path = path;

    [PublicAPI]
    public RepositoryBuilder IncludeOnlyProjectsFromSolutions(params string[] slnPaths)
    {
        _slnPaths.AddRange(slnPaths);
        return this;
    }
    
    [PublicAPI]
    public RepositoryBuilder ExcludeProject(string projectName)
    {
        _excludedProjects.Add(projectName);
        return this;
    }

    public RepositoryToAnalyze Build() => new(_path, _slnPaths, _excludedProjects.AsReadOnly());
}
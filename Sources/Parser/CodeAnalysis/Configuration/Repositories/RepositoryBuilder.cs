using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace P3Model.Parser.CodeAnalysis.Configuration.Repositories;

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
    public RepositoryBuilder ExcludeProjects(params string[] projects)
    {
        _excludedProjects.AddRange(projects);
        return this;
    }

    public Task<RepositoryToAnalyze> Build() => RepositoryToAnalyze.Load(_path, 
        _slnPaths.AsReadOnly(), 
        _excludedProjects.AsReadOnly());
}
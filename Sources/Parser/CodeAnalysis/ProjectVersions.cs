using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Serilog;

namespace P3Model.Parser.CodeAnalysis;

public class ProjectVersions
{
    private readonly string _name;
    private readonly IReadOnlyDictionary<TargetFramework, Project> _versions;

    private ProjectVersions(string name, IReadOnlyDictionary<TargetFramework, Project> versions)
    {
        _name = name;
        _versions = versions;
    }

    public static IEnumerable<ProjectVersions> CreateFor(IEnumerable<Project> projects) => projects
        .Select(ProjectVersion.Create)
        .GroupBy(version => version.Name)
        .Select(group => new ProjectVersions(group.Key,
            group.ToDictionary(v => v.TargetFramework, v => v.Project)));

    public Project? GetFor(TargetFramework? defaultFramework)
    {
        if (_versions.Count == 1)
            return _versions.Values.First();
        if (!defaultFramework.HasValue)
        {
            Log.Warning(
                $"Project {_name} has versions for multiple target frameworks, but no default framework was specified.");
            return null;
        }
        if (!_versions.TryGetValue(defaultFramework.Value, out var project))
        {
            Log.Warning(
                $"Project {_name} has no version for default target framework {defaultFramework}, versions are present for: {string.Join(", ", _versions.Keys)}.");
            return null;
        }
        return project;
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using Serilog;

namespace P3Model.Parser.CodeAnalysis;

public class RepositoryToAnalyze
{
    public DirectoryInfo Directory { get; }
    private readonly IReadOnlyCollection<Project> _projects;

    private RepositoryToAnalyze(DirectoryInfo directory, IReadOnlyCollection<Project> projects)
    {
        Directory = directory;
        _projects = projects;
    }

    public static async Task<RepositoryToAnalyze> Load(string path,
        IReadOnlyCollection<string> includedSolutions,
        IReadOnlyCollection<string> excludedProjects)
    {
        Log.Information($"Loading repository: {path} started.");
        var stopwatch = Stopwatch.StartNew();
        var directory = new DirectoryInfo(path);
        var workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += (_, args) => Log.Error(args.Diagnostic.Message);
        workspace.SkipUnrecognizedProjects = true;
        var projects = new HashSet<Project>(new ProjectComparer());
        foreach (var slnPath in GetAllSlnPaths(directory, includedSolutions))
        {
            var solution = await workspace.OpenSolutionAsync(slnPath);
            foreach (var project in solution.Projects.Where(p => !excludedProjects.Contains(p.Name)))
            {
                if (projects.Add(project))
                    Log.Verbose($"Project added to analysis: {project.Name}");
            }
        }
        stopwatch.Stop();
        Log.Information($"Loading repository: {path} finished in {stopwatch.ElapsedMilliseconds / 1000}s.");
        return new RepositoryToAnalyze(directory, projects);
    }

    private static IEnumerable<string> GetAllSlnPaths(DirectoryInfo directory,
        IReadOnlyCollection<string> includedSolutions)
    {
        if (includedSolutions.Count != 0)
            return includedSolutions;
        return directory
            .EnumerateFiles("*.sln", SearchOption.AllDirectories)
            .Select(fileInfo => fileInfo.FullName);
    }

    public IEnumerable<Project> GetProjectsFor(TargetFrameworks targetFrameworks)
    {
        if (targetFrameworks == TargetFrameworks.All)
            return _projects;
        throw new NotImplementedException();
    }

    private class ProjectComparer : IEqualityComparer<Project>
    {
        public bool Equals(Project? x, Project? y)
        {
            if (x == null || y == null)
                return false;
            return string.Equals(x.FilePath, y.FilePath, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Project obj) => string.IsNullOrEmpty(obj.FilePath) ? 0 : obj.FilePath.GetHashCode();
    }
}
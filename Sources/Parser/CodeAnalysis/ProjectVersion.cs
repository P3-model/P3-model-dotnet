using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis;

public record ProjectVersion(string Name, TargetFramework TargetFramework, Project Project)
{
    private static readonly Regex ProjectNameRegex = new(@"^(?<name>.+?)(\((?<targetFramework>.+?)\))?$");

    public static ProjectVersion Create(Project project)
    {
        var (name, targetFramework) = GetNameAndTargetFrameworkFor(project);
        return new ProjectVersion(name, targetFramework, project);
    }

    private static (string name, TargetFramework targetFramework) GetNameAndTargetFrameworkFor(Project project)
    {
        var projectNameMatch = ProjectNameRegex.Match(project.Name);
        if (!projectNameMatch.Success)
            throw new ParserError($"Unsupported project name format: {project.Name}");
        var projectNameGroup = projectNameMatch.Groups["name"];
        if (!projectNameGroup.Success)
            throw new ParserError($"Unsupported project name format: {project.Name}");
        var name = projectNameMatch.Groups["name"].Value;
        var targetFrameworkGroup = projectNameMatch.Groups["targetFramework"];
        var targetFramework = targetFrameworkGroup.Success
            ? new TargetFramework(targetFrameworkGroup.Value)
            : TargetFramework.Unspecified;
        return (name, targetFramework);
    }
}
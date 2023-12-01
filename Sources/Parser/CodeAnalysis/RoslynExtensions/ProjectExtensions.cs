using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

public static class ProjectExtensions
{
    public static TargetFramework GetTargetFramework(this Project project)
    {
        if (project.FilePath is null)
            throw new ParserError($"Can not find csproj file for project: {project.Name}");
        var xml = XDocument.Load(project.FilePath);
        var targetFrameworkElement = xml.Descendants("TargetFramework").FirstOrDefault();
        if (targetFrameworkElement is null)
        {
            var targetFrameworksElement = xml.Descendants("TargetFrameworks").FirstOrDefault();
            if (targetFrameworksElement is null)
                throw new ParserError($"Missing target framework information for project: {project.Name}");
            throw new ParserError("Targeting multiple frameworks is not supported");
        }
        return targetFrameworkElement.Value switch
        {
            "net6.0" => TargetFramework.Net60,
            "net7.0" => TargetFramework.Net70,
            _ => throw new ParserError($"Unknown target framework: {targetFrameworkElement.Value}")
        };
    }
}
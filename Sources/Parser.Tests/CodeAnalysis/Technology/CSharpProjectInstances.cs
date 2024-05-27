using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.Technology;

public static class CSharpProjectInstances
{
    public static readonly CSharpProject MainProject = new(
        ElementId.Create<CSharpProject>("TestSamples.MainProject"),
        "TestSamples.MainProject",
        string.Empty);

    public static readonly CSharpProject StartupProject = new(
        ElementId.Create<CSharpProject>("TestSamples.StartupProject"),
        "TestSamples.StartupProject",
        string.Empty);

    public static readonly CSharpProject ConsoleApp = new(
        ElementId.Create<CSharpProject>("TestSamples.ConsoleApp"),
        "TestSamples.ConsoleApp",
        string.Empty);

    public static readonly CSharpProject WebApplication = new(
        ElementId.Create<CSharpProject>("TestSamples.WebApplication"),
        "TestSamples.WebApplication",
        string.Empty);

    public static readonly CSharpProject WorkerService = new(
        ElementId.Create<CSharpProject>("TestSamples.WorkerService"),
        "TestSamples.WorkerService",
        string.Empty);
}
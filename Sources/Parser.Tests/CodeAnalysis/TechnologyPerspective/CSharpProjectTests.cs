using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpProjectTests
{
    [Test]
    public void AllProjectsArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new CSharpProject(
            ElementId.Create<CSharpProject>("TestSamples.MainProject"),
            "TestSamples.MainProject",
            string.Empty),
        new CSharpProject(
            ElementId.Create<CSharpProject>("TestSamples.StartupProject"),
            "TestSamples.StartupProject",
            string.Empty),
        new CSharpProject(
            ElementId.Create<CSharpProject>("TestSamples.ConsoleApp"),
            "TestSamples.ConsoleApp",
            string.Empty),
        new CSharpProject(
            ElementId.Create<CSharpProject>("TestSamples.WebApplication"),
            "TestSamples.WebApplication",
            string.Empty),
        new CSharpProject(
            ElementId.Create<CSharpProject>("TestSamples.WorkerService"),
            "TestSamples.WorkerService",
            string.Empty));
}
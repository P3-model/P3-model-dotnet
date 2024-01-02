using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpProjectTests
{
    [Test]
    public void AllProjectsArePresent() => ParserOutput.AssertExistOnly(
        new CSharpProject("TestSamples.MainProject", string.Empty),
        new CSharpProject("TestSamples.StartupProject", string.Empty),
        new CSharpProject("TestSamples.ConsoleApp", string.Empty),
        new CSharpProject("TestSamples.WebApplication", string.Empty),
        new CSharpProject("TestSamples.WorkerService", string.Empty));
}
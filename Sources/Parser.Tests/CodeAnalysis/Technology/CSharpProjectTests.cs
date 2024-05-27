using NUnit.Framework;
using static P3Model.Parser.Tests.CodeAnalysis.Technology.CSharpProjectInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.Technology;

[TestFixture]
public class CSharpProjectTests
{
    [Test]
    public void AllProjectsArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        MainProject,
        StartupProject,
        ConsoleApp,
        WebApplication,
        WorkerService
    );
}
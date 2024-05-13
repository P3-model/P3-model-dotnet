using NUnit.Framework;
using static P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective.CSharpProjectInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

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
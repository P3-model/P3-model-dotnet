using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpTypeTests
{
    [Test]
    public void AllTypesArePresent() => ParserOutput.AssertExistOnly(
        new CSharpType("TestSamples.MainProject.DomainModules.NotModule.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithCode.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithoutCode.NotModule.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithoutCode.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.WorkerService.Worker", "Worker", string.Empty));
}
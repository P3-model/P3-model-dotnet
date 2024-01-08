using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpTypeTests
{
    [Test]
    public void AllTypesArePresent() => ParserOutput.AssertExistOnly(
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleBuildingBlock", "SampleBuildingBlock", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleDddAggregate", "SampleDddAggregate", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleDddDomainService", "SampleDddDomainService", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleDddEntity", "SampleDddEntity", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleDddFactory", "SampleDddFactory", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleDddRepository", "SampleDddRepository", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleDddValueObject", "SampleDddValueObject", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainBuildingBlocks.SampleModule.SampleProcessStep", "SampleProcessStep", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.NotModule.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithCode.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithoutCode.NotModule.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.DomainModules.WithoutCode.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.WorkerService.Worker", "Worker", string.Empty));
}
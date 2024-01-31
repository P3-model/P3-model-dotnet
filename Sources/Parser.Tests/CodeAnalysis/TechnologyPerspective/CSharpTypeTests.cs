using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpTypeTests
{
    [Test]
    public void AllTypesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleBuildingBlock", "SampleBuildingBlock", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate", "SampleDddAggregate", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregateFromBaseClass", "SampleDddAggregateFromBaseClass", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService", "SampleDddDomainService", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity", "SampleDddEntity", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory", "SampleDddFactory", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository", "SampleDddRepository", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject", "SampleDddValueObject", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObjectFromBaseInterface", "SampleDddValueObjectFromBaseInterface", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleExternalSystemIntegration", "SampleExternalSystemIntegration", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleProcessStep", "SampleProcessStep", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainModules.NotModule.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainModules.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainModules.WithCode.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainModules.WithoutCode.NotModule.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.Domain.DomainModules.WithoutCode.WithCode.Class1", "Class1", string.Empty),
        new CSharpType("TestSamples.MainProject.NotDomain.NamespaceInfo", "NamespaceInfo", string.Empty),
        new CSharpType("TestSamples.MainProject.NotDomain.SampleExternalSystemHttpIntegration", "SampleExternalSystemHttpIntegration", string.Empty),
        new CSharpType("TestSamples.WorkerService.Worker", "Worker", string.Empty));
}
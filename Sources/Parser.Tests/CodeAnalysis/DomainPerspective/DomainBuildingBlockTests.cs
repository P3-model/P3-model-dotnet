using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainBuildingBlockTests
{
    [Test]
    public void AllBuildingBlocksArePresent() => ParserOutput.AssertExistOnly(
        new DomainBuildingBlock("Domain.DomainBuildingBlocks.SampleModule.SampleBuildingBlock", "SampleBuildingBlock"),
        new DddAggregate("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate", "SampleDddAggregate"),
        new DddDomainService("Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService", "SampleDddDomainService"),
        new DddEntity("Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity", "SampleDddEntity"),
        new DddFactory("Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory", "SampleDddFactory"),
        new DddRepository("Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository", "SampleDddRepository"),
        new DddValueObject("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject", "SampleDddValueObject"),
        new ProcessStep("Domain.DomainBuildingBlocks.SampleModule.SampleProcessStep", "SampleProcessStep"));
}
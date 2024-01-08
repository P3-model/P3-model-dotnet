using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainBuildingBlockTests
{
    [Test]
    public void AllBuildingBlocksArePresent() => ParserOutput.AssertExistOnly(
        new DomainBuildingBlock("DomainBuildingBlocks.SampleModule.SampleBuildingBlock", "SampleBuildingBlock"),
        new DddAggregate("DomainBuildingBlocks.SampleModule.SampleDddAggregate", "SampleDddAggregate"),
        new DddDomainService("DomainBuildingBlocks.SampleModule.SampleDddDomainService", "SampleDddDomainService"),
        new DddEntity("DomainBuildingBlocks.SampleModule.SampleDddEntity", "SampleDddEntity"),
        new DddFactory("DomainBuildingBlocks.SampleModule.SampleDddFactory", "SampleDddFactory"),
        new DddRepository("DomainBuildingBlocks.SampleModule.SampleDddRepository", "SampleDddRepository"),
        new DddValueObject("DomainBuildingBlocks.SampleModule.SampleDddValueObject", "SampleDddValueObject"));
}
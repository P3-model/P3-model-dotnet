using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainBuildingBlockTests
{
    [Test]
    public void AllBuildingBlocksArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new DomainBuildingBlock("Domain.DomainBuildingBlocks.SampleModule.SampleBuildingBlock", "SampleBuildingBlock"),
        new DddAggregate("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate", "SampleDddAggregate"),
        new DddAggregate("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregateFromBaseClass",
            "SampleDddAggregateFromBaseClass"),
        new DddDomainService("Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService",
            "SampleDddDomainService"),
        new DddEntity("Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity", "SampleDddEntity"),
        new DddFactory("Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory", "SampleDddFactory"),
        new DddRepository("Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository", "SampleDddRepository"),
        new DddValueObject("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject", "SampleDddValueObject"),
        new DddValueObject("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObjectFromBaseInterface",
            "SampleDddValueObjectFromBaseInterface"),
        new ProcessStep("Domain.DomainBuildingBlocks.SampleModule.SampleProcessStep", "SampleProcessStep"));

    [Test]
    public void AllDependsOnRelationsArePresent()
    {
        var aggregate = new DddAggregate("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate",
            "SampleDddAggregate");
        var domainService = new DddDomainService("Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService",
            "SampleDddDomainService");
        var entity = new DddEntity("Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity", "SampleDddEntity");
        var factory = new DddFactory("Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory", "SampleDddFactory");
        var processStep = new ProcessStep("Domain.DomainBuildingBlocks.SampleModule.SampleProcessStep",
            "SampleProcessStep");
        var repository = new DddRepository("Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository",
            "SampleDddRepository");
        var valueObject = new DddValueObject("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject",
            "SampleDddValueObject");
        ParserOutput.AssertModelContainsOnlyRelations(
            new DomainBuildingBlock.DependsOnBuildingBlock(aggregate, domainService),
            new DomainBuildingBlock.DependsOnBuildingBlock(processStep, aggregate),
            new DomainBuildingBlock.DependsOnBuildingBlock(processStep, domainService),
            new DomainBuildingBlock.DependsOnBuildingBlock(processStep, entity),
            new DomainBuildingBlock.DependsOnBuildingBlock(processStep, factory),
            new DomainBuildingBlock.DependsOnBuildingBlock(processStep, repository),
            new DomainBuildingBlock.DependsOnBuildingBlock(processStep, valueObject),
            new DomainBuildingBlock.DependsOnBuildingBlock(repository, aggregate));
    }
}
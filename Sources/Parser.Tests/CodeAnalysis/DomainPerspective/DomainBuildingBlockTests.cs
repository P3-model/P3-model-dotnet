using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainBuildingBlockTests
{
    [Test]
    public void AllBuildingBlocksArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new DomainBuildingBlock(
            ElementId.Create<DomainBuildingBlock>("Domain.DomainBuildingBlocks.SampleModule.SampleBuildingBlock"),
            "Sample Building Block"),
        new DddAggregate(
            ElementId.Create<DddAggregate>("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate"),
            "Sample Ddd Aggregate")
        {
            ShortDescription = """
                               *lorem ipsum* **dolor** sit amet ...
                               - item 1
                               - item 2
                               ... consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                               """
        },
        new DddAggregate(
            ElementId.Create<DddAggregate>("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregateFromBaseClass"),
            "Sample Ddd Aggregate From Base Class"),
        new DddDomainService(
            ElementId.Create<DddDomainService>("Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService"),
            "Sample Ddd Domain Service"),
        new DddEntity(
            ElementId.Create<DddEntity>("Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity"),
            "Sample Ddd Entity"),
        new DddFactory(
            ElementId.Create<DddFactory>("Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory"),
            "Sample Ddd Factory"),
        new DddRepository(
            ElementId.Create<DddRepository>("Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository"),
            "Sample Ddd Repository"),
        new DddValueObject(
            ElementId.Create<DddValueObject>("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject"),
            "Sample Ddd Value Object"),
        new DddValueObject(
            ElementId.Create<DddValueObject>(
                "Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObjectFromBaseInterface"),
            "Sample Ddd Value Object From Base Interface"),
        new ExternalSystemIntegration(
            ElementId.Create<ExternalSystemIntegration>(
                "Domain.DomainBuildingBlocks.SampleModule.SampleExternalSystemIntegration"),
            "Sample External System Integration"),
        new ProcessStep(
            ElementId.Create<ProcessStep>("Domain.DomainBuildingBlocks.SampleModule.SampleProcessStep"),
            "Sample Process Step"));

    [Test]
    public void AllDependsOnRelationsArePresent()
    {
        var aggregate = new DddAggregate(
            ElementId.Create<DddAggregate>("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate"),
            "Sample Ddd Aggregate");
        var domainService = new DddDomainService(
            ElementId.Create<DddDomainService>("Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService"),
            "Sample Ddd Domain Service");
        var entity = new DddEntity(
            ElementId.Create<DddEntity>("Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity"),
            "Sample Ddd Entity");
        var factory = new DddFactory(
            ElementId.Create<DddFactory>("Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory"),
            "Sample Ddd Factory");
        var processStep = new ProcessStep(
            ElementId.Create<ProcessStep>("Domain.DomainBuildingBlocks.SampleModule.SampleProcessStep"),
            "Sample Process Step");
        var repository = new DddRepository(
            ElementId.Create<DddRepository>("Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository"),
            "Sample Ddd Repository");
        var valueObject = new DddValueObject(
            ElementId.Create<DddValueObject>("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject"),
            "Sample Ddd Value Object");
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

    [Test]
    public void AllIntegratesRelationsArePresent() => ParserOutput.AssertModelContainsOnlyRelations(
        new ExternalSystemIntegration.Integrates(
            new ExternalSystemIntegration(
                ElementId.Create<ExternalSystemIntegration>(
                    "Domain.DomainBuildingBlocks.SampleModule.SampleExternalSystemIntegration"),
                "Sample External System Integration"),
            new ExternalSystem(
                ElementId.Create<ExternalSystem>("SampleExternalSystem"),
                "Sample External System")));
}
using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Technology;
using static P3Model.Parser.Tests.CodeAnalysis.Domain.DomainBuildingBlockInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.Domain;

[TestFixture]
public class DomainBuildingBlockTests
{
    [Test]
    public void AllBuildingBlocksArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        SampleDomainBuildingBlock,
        SampleDddAggregate,
        SampleDddAggregateFromBaseClass,
        SampleDddDomainService,
        SampleDddEntity,
        SampleDddFactory,
        SampleDddRepository,
        SampleDddValueObject,
        SampleDddValueObjectFromBaseInterface,
        SampleExternalSystemIntegration,
        SampleUseCase,
        BuildingBlockFromNotModuleNamespace,
        BuildingBlockFromSkippedNamespace);
    
    [Test]
    public void AllDependsOnRelationsArePresent()
    {
        ParserOutput.AssertModelContainsOnlyRelations(
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleDddAggregate, SampleDddDomainService),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleDddAggregate, SampleDddValueObject),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleUseCase, SampleDddAggregate),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleUseCase, SampleDddDomainService),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleUseCase, SampleDddEntity),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleUseCase, SampleDddFactory),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleUseCase, SampleDddRepository),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleUseCase, SampleDddValueObject),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleDddRepository, SampleDddAggregate),
            new DomainBuildingBlock.DependsOnBuildingBlock(SampleDddFactory, SampleDddEntity)
        );
    }

    [Test]
    public void AllIntegratesRelationsArePresent() => ParserOutput.AssertModelContainsOnlyRelations(
        new ExternalSystemIntegration.Integrates(
            SampleExternalSystemIntegration,
            new ExternalSystem(
                ElementId.Create<ExternalSystem>("SampleExternalSystem"),
                "Sample External System")));
}
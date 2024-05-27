using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Domain;
using static P3Model.Parser.Tests.CodeAnalysis.Domain.DomainBuildingBlockInstances;
using static P3Model.Parser.Tests.CodeAnalysis.Domain.DomainModuleInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.Domain;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        DomainModuleInstances.Domain,
        DomainBuildingBlocks,
        SampleModule,
        DomainModules,
        WithCode,
        WithCodeWithCode,
        WithoutCode,
        WithoutCodeWithCode);

    [Test]
    public void AllContainsDomainModuleRelationsArePresent() => ParserOutput.AssertModelContainsOnlyRelations(
        new DomainModule.ContainsDomainModule(DomainModuleInstances.Domain, DomainBuildingBlocks),
        new DomainModule.ContainsDomainModule(DomainModuleInstances.Domain, DomainModules),
        new DomainModule.ContainsDomainModule(DomainBuildingBlocks, SampleModule),
        new DomainModule.ContainsDomainModule(DomainModules, WithCode),
        new DomainModule.ContainsDomainModule(DomainModules, WithoutCode),
        new DomainModule.ContainsDomainModule(WithCode, WithCodeWithCode),
        new DomainModule.ContainsDomainModule(WithoutCode, WithoutCodeWithCode)
    );

    [Test]
    public void AllContainsBuildingBlockRelationsArePresent()
    {
        ParserOutput.AssertModelContainsOnlyRelations(
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDomainBuildingBlock),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddAggregate),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddAggregateFromBaseClass),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddDomainService),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddEntity),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddFactory),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddRepository),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddValueObject),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleDddValueObjectFromBaseInterface),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleExternalSystemIntegration),
            new DomainModule.ContainsBuildingBlock(SampleModule, SampleUseCase),
            new DomainModule.ContainsBuildingBlock(WithoutCode, BuildingBlockFromNotModuleNamespace),
            new DomainModule.ContainsBuildingBlock(WithCode, BuildingBlockFromSkippedNamespace)
        );
    }
}
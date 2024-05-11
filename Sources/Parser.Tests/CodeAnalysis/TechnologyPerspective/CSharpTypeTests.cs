using NUnit.Framework;
using static P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective.CSharpTypeInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpTypeTests
{
    [Test]
    public void AllTypesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        SampleBuildingBlock,
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
        Class1,
        Class1WithCode,
        Class1WithCodeWithCode,
        Class1WithoutCodeNotModule,
        Class1WithoutCodeWithCode,
        NamespaceInfo,
        SampleExternalSystemHttpIntegration,
        BuildingBlockFromNotModuleNamespace,
        BuildingBlockFromSkippedNamespace,
        Worker
    );
}
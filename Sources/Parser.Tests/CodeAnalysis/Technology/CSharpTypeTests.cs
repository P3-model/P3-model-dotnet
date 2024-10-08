using NUnit.Framework;
using static P3Model.Parser.Tests.CodeAnalysis.Technology.CSharpTypeInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.Technology;

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
        SampleSqlRepository,
        Worker
    );
}
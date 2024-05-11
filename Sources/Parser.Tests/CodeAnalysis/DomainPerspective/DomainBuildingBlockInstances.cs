using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Domain.Ddd;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

public static class DomainBuildingBlockInstances
{
    public static readonly DomainBuildingBlock SampleDomainBuildingBlock = new(
        ElementId.Create<DomainBuildingBlock>("Domain.DomainBuildingBlocks.SampleModule.SampleBuildingBlock"),
        "Sample Building Block");

    public static readonly DddAggregate SampleDddAggregate = new(
        ElementId.Create<DddAggregate>("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate"),
        "Sample Ddd Aggregate")
    {
        ShortDescription = """
                           *lorem ipsum* **dolor** sit amet ...
                           - item 1
                           - item 2
                           ... consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                           """
    };

    public static readonly DddAggregate SampleDddAggregateFromBaseClass = new(
        ElementId.Create<DddAggregate>("Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregateFromBaseClass"),
        "Sample Ddd Aggregate From Base Class");

    public static readonly DddDomainService SampleDddDomainService = new(
        ElementId.Create<DddDomainService>("Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService"),
        "Sample Ddd Domain Service");

    public static readonly DddEntity SampleDddEntity = new(
        ElementId.Create<DddEntity>("Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity"),
        "Sample Ddd Entity");

    public static readonly DddFactory SampleDddFactory = new(
        ElementId.Create<DddFactory>("Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory"),
        "Sample Ddd Factory");

    public static readonly DddRepository SampleDddRepository = new(
        ElementId.Create<DddRepository>("Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository"),
        "Sample Ddd Repository");

    public static readonly DddValueObject SampleDddValueObject = new(
        ElementId.Create<DddValueObject>("Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject"),
        "Sample Ddd Value Object");

    public static readonly DddValueObject SampleDddValueObjectFromBaseInterface = new(
        ElementId.Create<DddValueObject>(
            "Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObjectFromBaseInterface"),
        "Sample Ddd Value Object From Base Interface");

    public static readonly ExternalSystemIntegration SampleExternalSystemIntegration = new(
        ElementId.Create<ExternalSystemIntegration>(
            "Domain.DomainBuildingBlocks.SampleModule.SampleExternalSystemIntegration"),
        "Sample External System Integration");

    public static readonly UseCase SampleUseCase = new(
        ElementId.Create<UseCase>("Domain.DomainBuildingBlocks.SampleModule.SampleUseCase"),
        "Sample Use Case");

    public static readonly DomainBuildingBlock BuildingBlockFromNotModuleNamespace = new(
        ElementId.Create<DomainBuildingBlock>("Domain.DomainModules.WithoutCode.BuildingBlockFromNotModuleNamespace"),
        "Building Block From Not Module Namespace");
    
    public static readonly DomainBuildingBlock BuildingBlockFromSkippedNamespace = new(
        ElementId.Create<DomainBuildingBlock>("Domain.DomainModules.WithCode.BuildingBlockFromSkippedNamespace"),
        "Building Block From Skipped Namespace");
}
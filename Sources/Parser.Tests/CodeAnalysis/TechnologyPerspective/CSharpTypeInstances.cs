using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

public static class CSharpTypeInstances
{
    public static readonly CSharpType SampleBuildingBlock = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleBuildingBlock"),
        "SampleBuildingBlock",
        string.Empty);

    public static readonly CSharpType SampleDddAggregate = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregate"),
        "SampleDddAggregate",
        string.Empty);

    public static readonly CSharpType SampleDddAggregateFromBaseClass = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddAggregateFromBaseClass"),
        "SampleDddAggregateFromBaseClass",
        string.Empty);

    public static readonly CSharpType SampleDddDomainService = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddDomainService"),
        "SampleDddDomainService",
        string.Empty);

    public static readonly CSharpType SampleDddEntity = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddEntity"),
        "SampleDddEntity",
        string.Empty);

    public static readonly CSharpType SampleDddFactory = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddFactory"),
        "SampleDddFactory",
        string.Empty);

    public static readonly CSharpType SampleDddRepository = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddRepository"),
        "SampleDddRepository",
        string.Empty);

    public static readonly CSharpType SampleDddValueObject = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObject"),
        "SampleDddValueObject",
        string.Empty);

    public static readonly CSharpType SampleDddValueObjectFromBaseInterface = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleDddValueObjectFromBaseInterface"),
        "SampleDddValueObjectFromBaseInterface",
        string.Empty);

    public static readonly CSharpType SampleExternalSystemIntegration = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleExternalSystemIntegration"),
        "SampleExternalSystemIntegration",
        string.Empty);

    public static readonly CSharpType SampleUseCase = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule.SampleUseCase"),
        "SampleUseCase",
        string.Empty);

    public static readonly CSharpType Class1 = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.NotModule.WithCode.Class1"),
        "Class1",
        string.Empty);

    public static readonly CSharpType Class1WithCode = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.WithCode.Class1"),
        "Class1",
        string.Empty);

    public static readonly CSharpType Class1WithCodeWithCode = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.WithCode.WithCode.Class1"),
        "Class1",
        string.Empty);

    public static readonly CSharpType Class1WithoutCodeNotModule = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.WithoutCode.NotModule.Class1"),
        "Class1",
        string.Empty);

    public static readonly CSharpType Class1WithoutCodeWithCode = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.WithoutCode.WithCode.Class1"),
        "Class1",
        string.Empty);

    public static readonly CSharpType NamespaceInfo = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.NotDomain.NamespaceInfo"),
        "NamespaceInfo",
        string.Empty);

    public static readonly CSharpType SampleExternalSystemHttpIntegration = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.NotDomain.SampleExternalSystemHttpIntegration"),
        "SampleExternalSystemHttpIntegration",
        string.Empty);

    public static readonly CSharpType BuildingBlockFromNotModuleNamespace = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.WithoutCode.NotModule.BuildingBlockFromNotModuleNamespace"),
        "BuildingBlockFromNotModuleNamespace",
        string.Empty);
    
    public static readonly CSharpType BuildingBlockFromSkippedNamespace = new(
        ElementId.Create<CSharpType>("TestSamples.MainProject.Domain.DomainModules.NotModule.WithCode.SkippedWithAnnotation.BuildingBlockFromSkippedNamespace"),
        "BuildingBlockFromSkippedNamespace",
        string.Empty);

    public static readonly CSharpType Worker = new(
        ElementId.Create<CSharpType>("TestSamples.WorkerService.Worker"),
        "Worker",
        string.Empty);
}
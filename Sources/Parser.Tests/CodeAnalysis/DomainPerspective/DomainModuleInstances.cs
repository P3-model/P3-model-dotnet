using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

public static class DomainModuleInstances
{
    public static readonly DomainModule Domain = new(
        ElementId.Create<DomainModule>("Domain"),
        HierarchyPath.FromValue("Domain"));

    public static readonly DomainModule DomainBuildingBlocks = new(
        ElementId.Create<DomainModule>("Domain.DomainBuildingBlocks"),
        HierarchyPath.FromValue("Domain.DomainBuildingBlocks"));

    public static readonly DomainModule SampleModule = new(
        ElementId.Create<DomainModule>("Domain.DomainBuildingBlocks.SampleModule"),
        HierarchyPath.FromValue("Domain.DomainBuildingBlocks.SampleModule"));

    public static readonly DomainModule DomainModules = new(
        ElementId.Create<DomainModule>("Domain.DomainModules"),
        HierarchyPath.FromValue("Domain.DomainModules"));

    public static readonly DomainModule WithCode = new(
        ElementId.Create<DomainModule>("Domain.DomainModules.WithCode"),
        HierarchyPath.FromValue("Domain.DomainModules.WithCode"));

    public static readonly DomainModule WithCodeWithCode = new(
        ElementId.Create<DomainModule>("Domain.DomainModules.WithCode.WithCode"),
        HierarchyPath.FromValue("Domain.DomainModules.WithCode.WithCode"));

    public static readonly DomainModule WithoutCode = new(
        ElementId.Create<DomainModule>("Domain.DomainModules.WithoutCode"),
        HierarchyPath.FromValue("Domain.DomainModules.WithoutCode"));

    public static readonly DomainModule WithoutCodeWithCode = new(
        ElementId.Create<DomainModule>("Domain.DomainModules.WithoutCode.WithCode"),
        HierarchyPath.FromValue("Domain.DomainModules.WithoutCode.WithCode"));
}
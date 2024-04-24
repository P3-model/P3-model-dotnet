using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new DomainModule(
            ElementId.Create<DomainModule>("Domain"),
            HierarchyPath.FromValue("Domain")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainBuildingBlocks"),
            HierarchyPath.FromValue("Domain.DomainBuildingBlocks")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainBuildingBlocks.SampleModule"),
            HierarchyPath.FromValue("Domain.DomainBuildingBlocks.SampleModule")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules"),
            HierarchyPath.FromValue("Domain.DomainModules")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithCode")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithCode.WithCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithCode.WithCode")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithoutCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithoutCode")),
        new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithoutCode.WithCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithoutCode.WithCode"))
    );

    [Test]
    public void AllContainsDomainModuleRelationsArePresent()
    {
        var domain = new DomainModule(
            ElementId.Create<DomainModule>("Domain"),
            HierarchyPath.FromValue("Domain"));
        var domainBuildingBlocks = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainBuildingBlocks"),
            HierarchyPath.FromValue("Domain.DomainBuildingBlocks"));
        var domainBuildingBlocksSampleModule = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainBuildingBlocks.SampleModule"),
            HierarchyPath.FromValue("Domain.DomainBuildingBlocks.SampleModule"));
        var domainModules = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules"),
            HierarchyPath.FromValue("Domain.DomainModules"));
        var domainModulesWithCode = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithCode"));
        var domainModulesWithCodeWithCode = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithCode.WithCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithCode.WithCode"));
        var domainModulesWithoutCode = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithoutCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithoutCode"));
        var domainModulesWithoutCodeWithCode = new DomainModule(
            ElementId.Create<DomainModule>("Domain.DomainModules.WithoutCode.WithCode"),
            HierarchyPath.FromValue("Domain.DomainModules.WithoutCode.WithCode"));

        ParserOutput.AssertModelContainsOnlyRelations(
            new DomainModule.ContainsDomainModule(domain, domainBuildingBlocks),
            new DomainModule.ContainsDomainModule(domain, domainModules),
            new DomainModule.ContainsDomainModule(domainBuildingBlocks, domainBuildingBlocksSampleModule),
            new DomainModule.ContainsDomainModule(domainModules, domainModulesWithCode),
            new DomainModule.ContainsDomainModule(domainModules, domainModulesWithoutCode),
            new DomainModule.ContainsDomainModule(domainModulesWithCode, domainModulesWithCodeWithCode),
            new DomainModule.ContainsDomainModule(domainModulesWithoutCode, domainModulesWithoutCodeWithCode)
        );
    }
}
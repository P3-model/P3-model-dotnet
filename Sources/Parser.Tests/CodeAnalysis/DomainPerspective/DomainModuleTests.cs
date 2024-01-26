using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new DomainModule(HierarchyId.FromValue("Domain")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainBuildingBlocks")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainBuildingBlocks.SampleModule")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithCode")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithCode.WithCode")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithoutCode")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithoutCode.WithCode")));

    [Test]
    public void AllContainsDomainModuleRelationsArePresent()
    {
        var domain = new DomainModule(HierarchyId.FromValue("Domain"));
        var domainBuildingBlocks = new DomainModule(HierarchyId.FromValue("Domain.DomainBuildingBlocks"));
        var domainBuildingBlocksSampleModule =
            new DomainModule(HierarchyId.FromValue("Domain.DomainBuildingBlocks.SampleModule"));
        var domainModules = new DomainModule(HierarchyId.FromValue("Domain.DomainModules"));
        var domainModulesWithCode = new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithCode"));
        var domainModulesWithCodeWithCode =
            new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithCode.WithCode"));
        var domainModulesWithoutCode = new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithoutCode"));
        var domainModulesWithoutCodeWithCode =
            new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithoutCode.WithCode"));
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
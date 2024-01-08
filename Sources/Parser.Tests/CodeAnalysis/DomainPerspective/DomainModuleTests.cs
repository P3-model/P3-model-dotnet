using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertExistOnly(
        new DomainModule(HierarchyId.FromValue("Domain")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainBuildingBlocks")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainBuildingBlocks.SampleModule")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithCode")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithCode.WithCode")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithoutCode")),
        new DomainModule(HierarchyId.FromValue("Domain.DomainModules.WithoutCode.WithCode")));
}
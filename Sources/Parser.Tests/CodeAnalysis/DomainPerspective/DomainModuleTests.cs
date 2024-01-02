using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertExistOnly(
        new DomainModule(HierarchyId.FromValue("MainProject")),
        new DomainModule(HierarchyId.FromValue("MainProject.ModuleWithoutDirectCode")),
        new DomainModule(HierarchyId.FromValue("MainProject.ModuleWithoutDirectCode.ModuleWithDirectCode")),
        new DomainModule(HierarchyId.FromValue("MainProject.ModuleWithoutDirectCode.NotDomainModule.ModuleWithDirectCode")));
}
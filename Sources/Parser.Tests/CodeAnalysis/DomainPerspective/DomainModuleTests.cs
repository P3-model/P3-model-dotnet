using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertExistOnly(
        new DomainModule(HierarchyId.FromValue("DomainModules")),
        new DomainModule(HierarchyId.FromValue("DomainModules.WithCode")),
        new DomainModule(HierarchyId.FromValue("DomainModules.WithCode.WithCode")),
        new DomainModule(HierarchyId.FromValue("DomainModules.WithoutCode")),
        new DomainModule(HierarchyId.FromValue("DomainModules.WithoutCode.WithCode")));
}
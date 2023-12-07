using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class DomainModuleTests
{
    [Test]
    public void AllModulesArePresent() => ParserOutput.AssertExistOnly(
        new DomainModule(HierarchyId.FromValue("Module1")),
        new DomainModule(HierarchyId.FromValue("Module2")),
        new DomainModule(HierarchyId.FromValue("Module3")));
}
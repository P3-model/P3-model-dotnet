using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class CSharpProjectTests
{
    [Test]
    public void AllProjectsArePresent() => ParserOutput.AssertExistOnly(
        new CSharpProject("MyCompany.MySystem.Module1.Adapters", string.Empty),
        new CSharpProject("MyCompany.MySystem.Module1.Entities", string.Empty),
        new CSharpProject("MyCompany.MySystem.Module1.UseCases", string.Empty),
        new CSharpProject("MyCompany.MySystem.Module2.Api", string.Empty),
        new CSharpProject("MyCompany.MySystem.Module2.Infrastructure", string.Empty),
        new CSharpProject("MyCompany.MySystem.Module3", string.Empty),
        new CSharpProject("MyCompany.MySystem.ModularMonolith", string.Empty),
        new CSharpProject("MyCompany.MySystem.Service1", string.Empty),
        new CSharpProject("MyCompany.MySystem.Service1.PrivateLibrary", string.Empty),
        new CSharpProject("MyCompany.MySystem.Service2", string.Empty),
        new CSharpProject("MyCompany.MySystem.Service2.PrivateLibrary", string.Empty),
        new CSharpProject("MyCompany.MySystem.SharedLibrary1", string.Empty),
        new CSharpProject("MyCompany.MySystem.SharedLibrary2", string.Empty));
}
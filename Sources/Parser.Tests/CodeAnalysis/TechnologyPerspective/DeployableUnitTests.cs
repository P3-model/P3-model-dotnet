using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class DeployableUnitTests
{
    [Test]
    public void AllDeployableUnitsArePresent() => ParserOutput.AssertExistOnly(
        new DeployableUnit("MySystem-main-monolith"),
        new DeployableUnit("Module3-microservice"),
        new DeployableUnit("Service1"),
        new DeployableUnit("Service2"));
}
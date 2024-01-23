using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class DeployableUnitTests
{
    [Test]
    public void AllDeployableUnitsArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new DeployableUnit("main"),
        new DeployableUnit("console-app"),
        new DeployableUnit("web-app"),
        new DeployableUnit("worker-service"));
}
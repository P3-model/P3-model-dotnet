using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class DeployableUnitTests
{
    [Test]
    public void AllDeployableUnitsArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        new DeployableUnit(
            ElementId.Create<DeployableUnit>("main"),
            "main"),
        new DeployableUnit(
            ElementId.Create<DeployableUnit>("consoleapp"),
            "console-app"),
        new DeployableUnit(
            ElementId.Create<DeployableUnit>("webapp"),
            "web-app"),
        new DeployableUnit(
            ElementId.Create<DeployableUnit>("workerservice"),
            "worker-service"));
}
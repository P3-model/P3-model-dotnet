using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

public static class DeployableUnitInstances
{
    public static readonly DeployableUnit MainDeployableUnit = new(
        ElementId.Create<DeployableUnit>("main"),
        "main");

    public static readonly DeployableUnit ConsoleAppDeployableUnit = new(
        ElementId.Create<DeployableUnit>("consoleapp"),
        "console-app");

    public static readonly DeployableUnit WebAppDeployableUnit = new(
        ElementId.Create<DeployableUnit>("webapp"),
        "web-app");

    public static readonly DeployableUnit WorkerServiceDeployableUnit = new(
        ElementId.Create<DeployableUnit>("workerservice"),
        "worker-service");
}
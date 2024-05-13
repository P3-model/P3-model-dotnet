using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Technology;
using static P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective.CSharpProjectInstances;
using static P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective.DatabaseInstances;
using static P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective.DeployableUnitInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class DeployableUnitTests
{
    [Test]
    public void AllDeployableUnitsArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        MainDeployableUnit,
        ConsoleAppDeployableUnit,
        WebAppDeployableUnit,
        WorkerServiceDeployableUnit
    );
    
    [Test]
    public void AllContainsCSharpProjectRelationsArePresent() => ParserOutput.AssertModelContainsOnlyRelations(
        new DeployableUnit.ContainsCSharpProject(MainDeployableUnit, StartupProject),
        new DeployableUnit.ContainsCSharpProject(ConsoleAppDeployableUnit, ConsoleApp),
        new DeployableUnit.ContainsCSharpProject(WebAppDeployableUnit, WebApplication),
        new DeployableUnit.ContainsCSharpProject(WorkerServiceDeployableUnit, WorkerService)
    );

    [Test]
    public void AllUsesDatabaseRelationsArePresent() => ParserOutput.AssertModelContainsOnlyRelations(
        new DeployableUnit.UsesDatabase(MainDeployableUnit, MainDatabase));

}
using System.Collections;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.ModelSyntax.ExampleObjects;

public class ExampleRelations : IEnumerable<Relation>
{
    private readonly Dictionary<Type, Relation> _relations = new();

    public static readonly ExampleRelations All =
    [
        new Actor.UsesProcessStep(
            new Actor(
                ElementId.Create<Actor>("ExampleActor"), 
                "ExampleActor"),
            new ProcessStep(
                ElementId.Create<ProcessStep>("ExampleModuleA.StepA"), 
                "StepA")),
        new BusinessOrganizationalUnit.OwnsDomainModule(
            new BusinessOrganizationalUnit(
                ElementId.Create<BusinessOrganizationalUnit>("ExampleBusinessOrganizationalUnit"),
                "ExampleBusinessOrganizationalUnit"),
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA.ModuleB"),
                HierarchyPath.FromValue("ExampleModuleA.ModuleB"))),

        new CodeStructure.BelongsToLayer(
            new CSharpProject(
                ElementId.Create<CSharpProject>("ExampleCSharpProjectA"),
                "ExampleCSharpProjectA",
                "ExampleDirectory/ExampleCodeStructure"),
            new Layer(ElementId.Create<Layer>("ExampleLayer"), "ExampleLayer")),

        new CSharpProject.ReferencesProject(
            new CSharpProject(ElementId.Create<CSharpProject>("ExampleCSharpProjectA"),
                "ExampleCSharpProjectA",
                "ExampleDirectory/ExampleCSharpProjectA"),
            new CSharpProject(ElementId.Create<CSharpProject>("ExampleCSharpProjectB"),
                "ExampleCSharpProjectB",
                "ExampleDirectory/ExampleCSharpProjectB")),

        new CSharpProject.ContainsNamespace(
            new CSharpProject(ElementId.Create<CSharpProject>("ExampleCSharpProjectA"),
                "ExampleCSharpProjectA",
                "ExampleDirectory/ExampleCSharpProjectA"),
            new CSharpNamespace(
                ElementId.Create<CSharpNamespace>("ExampleCSharpNamespace"),
                HierarchyPath.FromValue("ExampleCSharpNamespace"),
                "ExampleDirectory/ExampleCSharpNamespace")),

        new CSharpNamespace.ContainsNamespace(
            new CSharpNamespace(
                ElementId.Create<CSharpNamespace>("ExampleCSharpNamespaceA"),
                HierarchyPath.FromValue("ExampleCSharpNamespaceA"),
                "ExampleDirectory/ExampleCSharpNamespaceA"),
            new CSharpNamespace(
                ElementId.Create<CSharpNamespace>("ExampleCSharpNamespaceB"),
                HierarchyPath.FromValue("ExampleCSharpNamespaceB"),
                "ExampleDirectory/ExampleCSharpNamespaceB")),

        new CSharpNamespace.ContainsType(
            new CSharpNamespace(
                ElementId.Create<CSharpNamespace>("ExampleCSharpNamespace"),
                HierarchyPath.FromValue("ExampleCSharpNamespace"), 
                "ExampleDirectory/"),
            new CSharpType(ElementId.Create<CSharpType>("ExampleCSharpNamespace.ExampleCSharpType"),
                "ExampleCSharpType",
                "ExampleDirectory/ExampleCSharpType")),

        new Database.BelongsToCluster(
            new Database(ElementId.Create<Database>("ExampleDatabase"), "ExampleDatabase"),
            new DatabaseCluster(ElementId.Create<DatabaseCluster>("ExampleDatabaseCluster"), "ExampleDatabaseCluster")),

        new DeployableUnit.ContainsCSharpProject(
            new DeployableUnit(ElementId.Create<DeployableUnit>("ExampleDeployableUnit"), "ExampleDeployableUnit"),
            new CSharpProject(ElementId.Create<CSharpProject>("ExampleCSharpProject"),
                "ExampleCSharpProject",
                "ExampleDirectory/ExampleCSharpProject")),

        new DeployableUnit.UsesDatabase(
            new DeployableUnit(ElementId.Create<DeployableUnit>("ExampleDeployableUnit"), "ExampleDeployableUnit"),
            new Database(ElementId.Create<Database>("ExampleDatabase"), "ExampleDatabase")),

        new DevelopmentTeam.OwnsDomainModule(
            new DevelopmentTeam(ElementId.Create<DevelopmentTeam>("ExampleDevelopmentTeam"), "ExampleDevelopmentTeam"),
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA.ModuleB"),
                HierarchyPath.FromValue("ExampleModuleA.ModuleB"))),

        new DomainBuildingBlock.DependsOnBuildingBlock(
            new DomainBuildingBlock(ElementId.Create<DomainBuildingBlock>("ExampleModuleA.ExampleDomainBuildingBlockA"),
                "ExampleDomainBuildingBlockA"),
            new DomainBuildingBlock(ElementId.Create<DomainBuildingBlock>("ExampleModuleA.ExampleDomainBuildingBlockB"),
                "ExampleDomainBuildingBlockB")),

        new DomainBuildingBlock.IsImplementedBy(
            new DomainBuildingBlock(ElementId.Create<DomainBuildingBlock>("ExampleModuleA.ExampleDomainBuildingBlockA"),
                "ExampleDomainBuildingBlockA"),
            new CSharpType(ElementId.Create<CSharpType>("ExampleCSharpNamespace.ExampleCSharpType"),
                "ExampleCSharpType",
                "ExampleDirectory/ExampleCSharpType")),

        new DomainModule.ContainsDomainModule(
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA"),
                HierarchyPath.FromValue("ExampleModuleA")),
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA.ModuleB"),
                HierarchyPath.FromValue("ExampleModuleA.ModuleB"))),

        new DomainModule.ContainsBuildingBlock(
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA"),
                HierarchyPath.FromValue("ExampleModuleA")),
            new DomainBuildingBlock(ElementId.Create<DomainBuildingBlock>("ExampleModuleA.ExampleDomainBuildingBlock"),
                "ExampleDomainBuildingBlock")),

        new DomainModule.IsDeployedInDeployableUnit(
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA"),
                HierarchyPath.FromValue("ExampleModuleA")),
            new DeployableUnit(ElementId.Create<DeployableUnit>("ExampleDeployableUnit"), "ExampleDeployableUnit")),

        new DomainModule.IsImplementedBy(
            new DomainModule(
                ElementId.Create<DomainModule>("ExampleModuleA"),
                HierarchyPath.FromValue("ExampleModuleA")),
            new CSharpNamespace(
                ElementId.Create<CSharpNamespace>("ExampleParentCSharpNamespace.ExampleCSharpNamespace"),
                HierarchyPath.FromValue("ExampleParentCSharpNamespace.ExampleCSharpNamespace"), 
                "ExampleDirectory/ExampleCSharpNamespace")),

        new ExternalSystemIntegration.Integrates(
            new ExternalSystemIntegration(
                ElementId.Create<ExternalSystemIntegration>("ExampleModuleA.ExternalSystemIntegration"),
                "ExternalSystemIntegration"),
            new ExternalSystem(ElementId.Create<ExternalSystem>("ExampleExternalSystem"), "ExampleExternalSystem")),

        new Process.ContainsProcessStep(
            new Process(ElementId.Create<Process>("ExampleProcess"), "ExampleProcess"),
            new ProcessStep(ElementId.Create<ProcessStep>("ExampleModuleA.StepA"), "StepA")),

        new ProcessStep.HasNextStep(
            new ProcessStep(ElementId.Create<ProcessStep>("ExampleModuleA.StepA"), "StepA"),
            new ProcessStep(ElementId.Create<ProcessStep>("ExampleModuleA.StepB"), "StepB")),

        new ProcessStep.DependsOnBuildingBlock(
            new ProcessStep(ElementId.Create<ProcessStep>("ExampleModuleA.StepZ"), "StepZ"),
            new DomainBuildingBlock(ElementId.Create<DomainBuildingBlock>("ExampleModuleA.ExampleBuildingBlock"),
                "ExampleBuildingBlock")),

        new Tier.ContainsDeployableUnit(
            new Tier(ElementId.Create<Tier>("ExampleTier"), "ExampleTier"),
            new DeployableUnit(ElementId.Create<DeployableUnit>("ExampleDeployableUnit"), "ExampleDeployableUnit"))
    ];

    private void Add<TRelation>(TRelation relation)
        where TRelation : class, Relation
        => _relations.Add(typeof(TRelation), relation);

    public Relation ForType(Type type)
    {
        if (!_relations.TryGetValue(type, out var relation))
            throw new InvalidOperationException($"Missing example for {type.Name}");
        return relation;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Relation> GetEnumerator() => _relations.Values.GetEnumerator();
}
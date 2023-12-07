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

    private static readonly DomainModule DomainModule =
        new(HierarchyId.FromParts("ExampleModuleA", "ModuleB", "ModuleC"));
    
    public static readonly ExampleRelations All = new()
    {
        new Actor.UsesProcessStep(
            new Actor("ExampleActor"),
            new ProcessStep(DomainModule, "StepA")),
        new BusinessOrganizationalUnit.OwnsDomainModule(
            new BusinessOrganizationalUnit("ExampleBusinessOrganizationalUnit"),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB"))),
        new CodeStructure.BelongsToLayer(
            new CSharpProject("ExampleCSharpProjectA", "ExampleDirectory/ExampleCodeStructure"),
            new Layer("ExampleLayer")),
        new CSharpProject.ReferencesProject(
            new CSharpProject("ExampleCSharpProjectA", "ExampleDirectory/ExampleCSharpProjectA"),
            new CSharpProject("ExampleCSharpProjectB", "ExampleDirectory/ExampleCSharpProjectB")),
        new CSharpProject.ContainsNamespace(
            new CSharpProject("ExampleCSharpProjectA", "ExampleDirectory/ExampleCSharpProjectA"),
            new CSharpNamespace(HierarchyId.FromParts("ExampleCSharpNamespace"), 
                "ExampleDirectory/ExampleCSharpNamespace")),
        new CSharpNamespace.ContainsNamespace(
            new CSharpNamespace(HierarchyId.FromParts("ExampleCSharpNamespaceA"), 
                "ExampleDirectory/ExampleCSharpNamespaceA"),
            new CSharpNamespace(HierarchyId.FromParts("ExampleCSharpNamespaceB"), 
                "ExampleDirectory/ExampleCSharpNamespaceB")),
        new CSharpNamespace.ContainsType(
            new CSharpNamespace(HierarchyId.FromParts("ExampleCSharpNamespace"), "ExampleDirectory/"),
            new CSharpType(HierarchyId.FromParts("ExampleCSharpType"),
                "ExampleCSharpType",
                "ExampleDirectory/ExampleCSharpType")),
        new Database.BelongsToCluster(
            new Database("ExampleDatabase"), 
            new DatabaseCluster("ExampleDatabaseCluster")),
        new DeployableUnit.ContainsCSharpProject(
            new DeployableUnit("ExampleDeployableUnit"),
            new CSharpProject("ExampleCSharpProject", "ExampleDirectory/ExampleCSharpProject")),
        new DeployableUnit.UsesDatabase(
            new DeployableUnit("ExampleDeployableUnit"),
            new Database("ExampleDatabase")),
        new DevelopmentTeam.OwnsDomainModule(
            new DevelopmentTeam("ExampleDevelopmentTeam"),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB"))),
        new DomainBuildingBlock.DependsOnBuildingBlock(
            new DomainBuildingBlock(DomainModule, "ExampleDomainBuildingBlockA"),
            new DomainBuildingBlock(DomainModule, "ExampleDomainBuildingBlockB")),
        new DomainBuildingBlock.IsImplementedBy(
            new DomainBuildingBlock(DomainModule, "ExampleDomainBuildingBlockA"),
            new CSharpType(HierarchyId.FromParts("ExampleCSharpNamespace", "ExampleCSharpType"),
                "ExampleCSharpType",
                "ExampleDirectory/ExampleCSharpType")),
        new DomainModule.ContainsDomainModule(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB"))),
        new DomainModule.ContainsBuildingBlock(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new DomainBuildingBlock(DomainModule, "ExampleDomainBuildingBlock")),
        new DomainModule.IsDeployedInDeployableUnit(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new DeployableUnit("ExampleDeployableUnit")),
        new DomainModule.IsImplementedBy(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new CSharpNamespace(HierarchyId.FromParts("ExampleParentCSharpNamespace", "ExampleCSharpNamespace"),
                "ExampleDirectory/ExampleCSharpNamespace")),
        new Process.ContainsProcessStep(
            new Process("ExampleProcess"),
            new ProcessStep(DomainModule, "StepA")),
        new ProcessStep.HasNextStep(
            new ProcessStep(DomainModule, "StepA"),
            new ProcessStep(DomainModule, "StepB")),
            new ProcessStep.DependsOnBuildingBlock(
            new ProcessStep(DomainModule, "StepZ"),
            new DomainBuildingBlock(DomainModule, "ExampleBuildingBlock")),
        new Tier.ContainsDeployableUnit(
            new Tier("ExampleTier"),
            new DeployableUnit("ExampleDeployableUnit")),
    };
    
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
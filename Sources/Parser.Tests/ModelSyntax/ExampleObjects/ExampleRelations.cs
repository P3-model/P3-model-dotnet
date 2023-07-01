using System.Collections;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace Parser.Tests.ModelSyntax.ExampleObjects;

public class ExampleRelations : IEnumerable<Relation>
{
    private readonly Dictionary<Type, Relation> _relations = new();

    public static readonly ExampleRelations All = new()
    {
        new Actor.UsesProduct(
            new Actor("ExampleActor"),
            new Product("ExampleProduct")),
        new Actor.UsesProcessStep(
            new Actor("ExampleActor"),
            new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepA"))),
        new BusinessOrganizationalUnit.OwnsDomainModule(
            new BusinessOrganizationalUnit("ExampleBusinessOrganizationalUnit"),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB"))),
        new DeployableUnit.ContainsCodeStructure(
            new DeployableUnit("ExampleDeployableUnit"),
            new CodeStructure("ExampleCodeStructure")),
        new DevelopmentTeam.OwnsDomainModule(
            new DevelopmentTeam("ExampleDevelopmentTeam"),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB"))),
        new DomainBuildingBlock.DependsOnBuildingBlock(
            new DomainBuildingBlock("ExampleDomainBuildingBlockA", null),
            new DomainBuildingBlock("ExampleDomainBuildingBlockB", null)),
        new DomainModule.ContainsDomainModule(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB"))),
        new DomainModule.ContainsBuildingBlock(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new DomainBuildingBlock("ExampleDomainBuildingBlock", null)),
        new DomainModule.IsDeployedInDeployableUnit(
            new DomainModule(HierarchyId.FromParts("ExampleModuleA")),
            new DeployableUnit("ExampleDeployableUnit")),
        new ExternalSystem.UsesProduct(
            new ExternalSystem("ExampleExternalSystem"),
            new Product("ExampleProduct")),
        new Process.ContainsProcessStep(
            new Process(HierarchyId.FromParts("ExampleProcessX", "ProcessY")),
            new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepA"))),
        new Process.ContainsSubProcess(
            new Process(HierarchyId.FromParts("ExampleProcessX")),
            new Process(HierarchyId.FromParts("ExampleProcessX", "ProcessY"))),
        new Process.HasNextSubProcess(
            new Process(HierarchyId.FromParts("ExampleProcessX", "ProcessY")),
            new Process(HierarchyId.FromParts("ExampleProcessX", "ProcessZ"))),
        new ProcessStep.HasNextStep(
            new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepA")),
            new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepB"))),
        new ProcessStep.BelongsToDomainModule(
            new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepA")),
            new DomainModule(HierarchyId.FromParts("ExampleModuleA"))),
        new ProcessStep.DependsOnBuildingBlock(
            new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepZ")),
            new DomainBuildingBlock("ExampleBuildingBlock", null)),
        new Product.UsesExternalSystem(
            new Product("ExampleProduct"),
            new ExternalSystem("ExampleExternalSystem")),
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
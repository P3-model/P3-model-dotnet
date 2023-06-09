using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record DomainModule(HierarchyId ModulesHierarchy) : Element
{
    public string Name => ModulesHierarchy.Last;

    public int Level => ModulesHierarchy.Level;

    public record ContainsDomainModule(DomainModule Parent, DomainModule Child) : Relation;

    public record ContainsBuildingBlock(DomainModule DomainModule, DomainBuildingBlock BuildingBlock) : Relation;
    
    public record ContainsProcessStep(DomainModule DomainModule, ProcessStep ProcessStep) : Relation;
}
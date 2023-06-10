using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record DomainModule(HierarchyId Hierarchy) : Element
{
    public string Name => Hierarchy.Name;

    public int Level => Hierarchy.Level;

    public record ContainsDomainModule(DomainModule Parent, DomainModule Child) : Relation;

    public record ContainsBuildingBlock(DomainModule DomainModule, DomainBuildingBlock BuildingBlock) : Relation;
    
    public record ContainsProcessStep(DomainModule DomainModule, ProcessStep ProcessStep) : Relation;
}
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record DomainModule(HierarchyId Hierarchy) : Element
{
    public string Name => Hierarchy.Name;

    public int Level => Hierarchy.Level;

    public record ContainsDomainModule(DomainModule Source, DomainModule Destination) 
        : Relation<DomainModule, DomainModule>;

    public record ContainsBuildingBlock(DomainModule Source, DomainBuildingBlock Destination) 
        : Relation<DomainModule, DomainBuildingBlock>;

    public record IsDeployedInDeployableUnit(DomainModule Source, DeployableUnit Destination) 
        : Relation<DomainModule, DeployableUnit>;
}
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record DomainBuildingBlock(DomainModule? Module, string Name) : Element
{
    public Perspective Perspective => Perspective.Domain;    
    public string Id => Module == null ? Name : $"{Module.Id}.{Name}";

    public record DependsOnBuildingBlock(DomainBuildingBlock Source, DomainBuildingBlock Destination)
        : Relation<DomainBuildingBlock, DomainBuildingBlock>;
    
    public record IsImplementedBy(DomainBuildingBlock Source, CodeStructure Destination)
        : Relation<DomainBuildingBlock, CodeStructure>;
}
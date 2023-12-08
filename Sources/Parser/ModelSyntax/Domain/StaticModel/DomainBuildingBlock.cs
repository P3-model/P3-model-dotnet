using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainBuildingBlock : ElementBase
{
    public override Perspective Perspective => Perspective.Domain;

    public DomainBuildingBlock(string name) : base(name) { }
    public DomainBuildingBlock(string id, string name) : base(id, name) { }

    public record DependsOnBuildingBlock(DomainBuildingBlock Source, DomainBuildingBlock Destination)
        : Relation<DomainBuildingBlock, DomainBuildingBlock>;
    
    public record IsImplementedBy(DomainBuildingBlock Source, CodeStructure Destination)
        : Relation<DomainBuildingBlock, CodeStructure>;
}
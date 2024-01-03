using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainBuildingBlock(string idPartUniqueForElementType, string name)
    : ElementBase(idPartUniqueForElementType, name)
{
    public override Perspective Perspective => Perspective.Domain;

    public record DependsOnBuildingBlock(DomainBuildingBlock Source, DomainBuildingBlock Destination)
        : Relation<DomainBuildingBlock, DomainBuildingBlock>;
    
    public record IsImplementedBy(DomainBuildingBlock Source, CodeStructure Destination)
        : Relation<DomainBuildingBlock, CodeStructure>;
}
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainBuildingBlock(string idPartUniqueForElementType, string name)
    : ElementBase(idPartUniqueForElementType, name)
{
    public override Perspective Perspective => Perspective.Domain;

    public class DependsOnBuildingBlock(DomainBuildingBlock source, DomainBuildingBlock destination)
        : RelationBase<DomainBuildingBlock, DomainBuildingBlock>(source, destination);
    
    public class IsImplementedBy(DomainBuildingBlock source, CodeStructure destination)
        : RelationBase<DomainBuildingBlock, CodeStructure>(source, destination);
}
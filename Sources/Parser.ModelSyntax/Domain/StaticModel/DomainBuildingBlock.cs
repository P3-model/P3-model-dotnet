using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainBuildingBlock(ElementId id, string name) : ElementBase(id, name)
{
    public override Perspective Perspective => Perspective.Domain;

    private string? _shortDescription;
    public string? ShortDescription
    {
        get => _shortDescription;
        set => SetOnce(ref _shortDescription, value);
    }

    public override bool DataEquals(Element? other) =>
        base.DataEquals(other) && 
        other is DomainBuildingBlock otherDomainBuildingBlock &&
        ShortDescription == otherDomainBuildingBlock.ShortDescription;

    public class DependsOnBuildingBlock(DomainBuildingBlock source, DomainBuildingBlock destination)
        : RelationBase<DomainBuildingBlock, DomainBuildingBlock>(source, destination);

    public class IsImplementedBy(DomainBuildingBlock source, CodeStructure destination)
        : RelationBase<DomainBuildingBlock, CodeStructure>(source, destination);
}
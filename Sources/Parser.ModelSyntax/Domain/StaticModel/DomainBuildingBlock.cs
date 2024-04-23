using System.Text.Json.Serialization;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainBuildingBlock(string idPartUniqueForElementType, string name)
    : ElementBase(idPartUniqueForElementType, name)
{
    public override Perspective Perspective => Perspective.Domain;

    public Attributes AdditionalAttributes { get; } = new();
    
    [JsonIgnore]
    public string? ShortDescription
    {
        get => AdditionalAttributes.ShortDescription;
        set => SetOnce(ref AdditionalAttributes.ShortDescription, value, Id);
    }

    public override bool DataEquals(Element? other) =>
        base.DataEquals(other) &&
        other is DomainBuildingBlock otherBuildingBlock &&
        ShortDescription == otherBuildingBlock.ShortDescription;

    public class Attributes
    {
        public string? ShortDescription;
    }
    
    public class DependsOnBuildingBlock(DomainBuildingBlock source, DomainBuildingBlock destination)
        : RelationBase<DomainBuildingBlock, DomainBuildingBlock>(source, destination);

    public class IsImplementedBy(DomainBuildingBlock source, CodeStructure destination)
        : RelationBase<DomainBuildingBlock, CodeStructure>(source, destination);
}
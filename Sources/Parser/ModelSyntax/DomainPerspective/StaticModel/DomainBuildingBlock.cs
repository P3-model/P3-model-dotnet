namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

// TODO: unique identification
public record DomainBuildingBlock(string Name) : Element
{
    public Perspective Perspective => Perspective.Domain;

    public record DependsOnBuildingBlock(DomainBuildingBlock Source, DomainBuildingBlock Destination)
        : Relation<DomainBuildingBlock, DomainBuildingBlock>;
}
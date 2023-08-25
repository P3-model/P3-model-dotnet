namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record DomainBuildingBlock(DomainModule? Module, string Name) : Element
{
    public Perspective Perspective => Perspective.Domain;

    public record DependsOnBuildingBlock(DomainBuildingBlock Source, DomainBuildingBlock Destination)
        : Relation<DomainBuildingBlock, DomainBuildingBlock>;
}
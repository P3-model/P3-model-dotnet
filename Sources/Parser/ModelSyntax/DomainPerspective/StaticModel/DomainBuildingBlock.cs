using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

// TODO: unique identification
public record DomainBuildingBlock(string Name, FileInfo? DescriptionFile) : Element
{
    public Perspective Perspective => Perspective.Domain;
    
    public virtual bool Equals(DomainBuildingBlock? other) => other != null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();

    public record DependsOnBuildingBlock(DomainBuildingBlock Source, DomainBuildingBlock Destination)
        : Relation<DomainBuildingBlock, DomainBuildingBlock>;
}
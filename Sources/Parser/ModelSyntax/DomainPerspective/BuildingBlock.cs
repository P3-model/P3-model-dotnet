using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public record BuildingBlock(string Name, FileInfo? DescriptionFile) : Element
{
    public virtual bool Equals(BuildingBlock? other) => other != null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();
}
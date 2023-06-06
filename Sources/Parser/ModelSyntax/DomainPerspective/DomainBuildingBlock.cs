using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public record DomainBuildingBlock(string Name, FileInfo? DescriptionFile) : Element
{
    public virtual bool Equals(DomainBuildingBlock? other) => other != null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();
}
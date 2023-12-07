using System;
using System.IO;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public record DomainBuildingBlockDescription(DomainBuildingBlock Element, FileInfo DescriptionFile)
    : Trait<DomainBuildingBlock>
{
    public virtual bool Equals(DomainBuildingBlockDescription? other) => 
        other != null && Element.Equals(other.Element) && 
        DescriptionFile.FullName.Equals(other.DescriptionFile.FullName);

    public override int GetHashCode() => HashCode.Combine(Element, DescriptionFile.FullName);
}
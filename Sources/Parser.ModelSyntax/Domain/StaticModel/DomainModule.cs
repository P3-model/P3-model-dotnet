using Humanizer;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainModule(ElementId id, HierarchyPath hierarchyPath) : 
    ElementBase(id, hierarchyPath.LastPart.Humanize(LetterCasing.Title)), 
    HierarchyElement
{
    public override Perspective Perspective => Perspective.Domain;
    public HierarchyPath HierarchyPath => hierarchyPath;

    public override bool DataEquals(Element? other) =>
        base.DataEquals(other) &&
        other is DomainModule otherDomainModule &&
        HierarchyPath.Equals(otherDomainModule.HierarchyPath);

    public override string ToString() => $"{base.ToString()} | HierarchyPath: {HierarchyPath}";

    public class ContainsDomainModule(DomainModule source, DomainModule destination)
        : HierarchyRelation<DomainModule>(source, destination);

    public class ContainsBuildingBlock(DomainModule source, DomainBuildingBlock destination)
        : RelationBase<DomainModule, DomainBuildingBlock>(source, destination);

    public class IsDeployedInDeployableUnit(DomainModule source, DeployableUnit destination)
        : RelationBase<DomainModule, DeployableUnit>(source, destination);

    public class IsImplementedBy(DomainModule source, CodeStructure destination)
        : RelationBase<DomainModule, CodeStructure>(source, destination);
}
using System.Linq;
using System.Text.Json.Serialization;
using Humanizer;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class DomainModule : ElementBase, HierarchyElement
{
    public override Perspective Perspective => Perspective.Domain;
    
    [JsonIgnore]
    public new HierarchyId Id { get; }

    [JsonIgnore]
    public string FullName => string.Join(" / ", Id.Parts.Select(p => p.Humanize(LetterCasing.Title)));

    [JsonIgnore]
    public int Level => Id.Level;
    
    public DomainModule(HierarchyId id) : base(id.Full, id.LastPart.Humanize(LetterCasing.Title)) => Id = id;

    public class ContainsDomainModule(DomainModule source, DomainModule destination) 
        : HierarchyRelation<DomainModule>(source, destination);

    public class ContainsBuildingBlock(DomainModule source, DomainBuildingBlock destination) 
        : RelationBase<DomainModule, DomainBuildingBlock>(source, destination);

    public class IsDeployedInDeployableUnit(DomainModule source, DeployableUnit destination) 
        : RelationBase<DomainModule, DeployableUnit>(source, destination);
    
    public class IsImplementedBy(DomainModule source, CodeStructure destination)
        : RelationBase<DomainModule, CodeStructure>(source, destination);
}
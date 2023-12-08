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

    public record ContainsDomainModule(DomainModule Source, DomainModule Destination) 
        : HierarchyRelation<DomainModule>;

    public record ContainsBuildingBlock(DomainModule Source, DomainBuildingBlock Destination) 
        : Relation<DomainModule, DomainBuildingBlock>;

    public record IsDeployedInDeployableUnit(DomainModule Source, DeployableUnit Destination) 
        : Relation<DomainModule, DeployableUnit>;
    
    public record IsImplementedBy(DomainModule Source, CodeStructure Destination)
        : Relation<DomainModule, CodeStructure>;
}
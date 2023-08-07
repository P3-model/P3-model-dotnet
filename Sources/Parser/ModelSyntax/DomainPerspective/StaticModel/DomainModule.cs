using System.Linq;
using System.Text.Json.Serialization;
using Humanizer;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record DomainModule(HierarchyId Id) : HierarchyElement
{
    public Perspective Perspective => Perspective.Domain;
    
    [JsonIgnore]
    public string Name => Id.LastPart.Humanize(LetterCasing.Title);
    
    [JsonIgnore]
    public string FullName => string.Join(" / ", Id.Parts.Select(p => p.Humanize(LetterCasing.Title)));

    [JsonIgnore]
    public int Level => Id.Level;

    public record ContainsDomainModule(DomainModule Source, DomainModule Destination) 
        : HierarchyRelation<DomainModule>;

    public record ContainsBuildingBlock(DomainModule Source, DomainBuildingBlock Destination) 
        : Relation<DomainModule, DomainBuildingBlock>;
    
    public record ContainsProcessStep(DomainModule Source, ProcessStep Destination) 
        : Relation<DomainModule, ProcessStep>;

    public record IsDeployedInDeployableUnit(DomainModule Source, DeployableUnit Destination) 
        : Relation<DomainModule, DeployableUnit>;
}
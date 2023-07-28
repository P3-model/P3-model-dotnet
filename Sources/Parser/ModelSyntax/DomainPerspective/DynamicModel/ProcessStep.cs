using System.Text.Json.Serialization;
using Humanizer;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public record ProcessStep(HierarchyId Id) : Element
{
    public Perspective Perspective => Perspective.Domain;
    
    [JsonIgnore]
    public string Name => Id.LastPart.Humanize(LetterCasing.Title);
    
    public record HasNextStep(ProcessStep Source, ProcessStep Destination) : Relation<ProcessStep, ProcessStep>;
    
    public record DependsOnBuildingBlock(ProcessStep Source, DomainBuildingBlock Destination)
        : Relation<ProcessStep, DomainBuildingBlock>;
}
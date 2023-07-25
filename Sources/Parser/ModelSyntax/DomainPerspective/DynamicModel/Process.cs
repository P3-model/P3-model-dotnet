using System.Text.Json.Serialization;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public record Process(HierarchyId Id) : HierarchyElement
{
    public Perspective Perspective => Perspective.Domain;

    [JsonIgnore]
    public string Name => Id.LastPart;

    public record ContainsSubProcess(Process Source, Process Destination) : HierarchyRelation<Process>;

    public record ContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;

    public record HasNextSubProcess(Process Source, Process Destination) : Relation<Process, Process>;
}
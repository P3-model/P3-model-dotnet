namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public record Process(HierarchyId Id) : HierarchyElement
{
    public string Name => Id.Name;

    public record ContainsSubProcess(Process Source, Process Destination) : HierarchyRelation<Process>;

    public record ContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;

    public record HasNextSubProcess(Process Source, Process Destination) : Relation<Process, Process>;
}
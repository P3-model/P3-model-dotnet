namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public record Process(HierarchyId Hierarchy) : Element
{
    public string Name => Hierarchy.Name;

    public record ContainsSubProcess(Process Source, Process Destination) : Relation<Process, Process>;

    public record ContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;
    
    public record DirectlyContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;

    public record HasNextSubProcess(Process Source, Process Destination) : Relation<Process, Process>;
}
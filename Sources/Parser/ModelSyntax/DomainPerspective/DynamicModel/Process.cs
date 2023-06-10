namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public record Process(HierarchyId Hierarchy) : Element
{
    public string Name => Hierarchy.Name;

    public record ContainsSubProcess(Process Parent, Process Child) : Relation;

    public record ContainsProcessStep(Process Process, ProcessStep Step) : Relation;

    public record HasNextSubProcess(Process Current, Process Next) : Relation;
}
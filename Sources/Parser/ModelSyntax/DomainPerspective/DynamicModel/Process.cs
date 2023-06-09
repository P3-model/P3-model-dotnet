namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

// TODO: unique names across hierarchy
public record Process(string Name) : Element
{
    public record ContainsSubProcess(Process Parent, Process Child) : Relation;
    public record ContainsProcessStep(Process Process, ProcessStep Step) : Relation;
}
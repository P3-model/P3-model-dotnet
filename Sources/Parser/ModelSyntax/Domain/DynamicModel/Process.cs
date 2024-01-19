using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.Domain.DynamicModel;

public class Process(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Domain;

    public class ContainsProcessStep(Process source, ProcessStep destination) 
        : RelationBase<Process, ProcessStep>(source, destination);
}
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class Actor(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.People;

    public class UsesProcessStep(Actor source, ProcessStep destination) 
        : RelationBase<Actor, ProcessStep>(source, destination);
}
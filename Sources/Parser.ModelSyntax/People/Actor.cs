using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.ModelSyntax.People;

public class Actor(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.People;

    public class UsesUseCase(Actor source, UseCase destination) 
        : RelationBase<Actor, UseCase>(source, destination);
}
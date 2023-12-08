using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class Actor : ElementBase
{
    public override Perspective Perspective => Perspective.People;

    public Actor(string name) : base(name) { }
    public Actor(string id, string name) : base(id, name) { }

    public record UsesProcessStep(Actor Source, ProcessStep Destination) : Relation<Actor, ProcessStep>;
}
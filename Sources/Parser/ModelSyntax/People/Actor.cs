using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class Actor(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.People;

    public record UsesProcessStep(Actor Source, ProcessStep Destination) : Relation<Actor, ProcessStep>;
}
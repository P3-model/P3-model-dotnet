using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.ModelSyntax.People;

public record Actor(string Name) : Element
{
    public Perspective Perspective => Perspective.People;
    
    public record UsesProcessStep(Actor Source, ProcessStep Destination) : Relation<Actor, ProcessStep>;
}
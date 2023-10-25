using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public record Actor(string Name) : Element
{
    public Perspective Perspective => Perspective.People;
    public string Id => Name;
    
    public record UsesProcessStep(Actor Source, ProcessStep Destination) : Relation<Actor, ProcessStep>;
}
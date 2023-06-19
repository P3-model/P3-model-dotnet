using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.ModelSyntax.People;

public record Actor(string Name) : Element
{
    public record UsesProduct(Actor Source, Product Destination) : Relation<Actor, Product>;
    
    public record UsesProcessStep(Actor Source, ProcessStep Destination) : Relation<Actor, ProcessStep>;
}
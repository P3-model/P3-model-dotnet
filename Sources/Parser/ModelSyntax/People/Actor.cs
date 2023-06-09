using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.ModelSyntax.People;

public record Actor(string Name) : Element
{
    public record UsesProduct(Actor Actor, Product Product) : Relation;
    
    public record UsesProcessStep(Actor Actor, ProcessStep ProcessStep) : Relation;
}
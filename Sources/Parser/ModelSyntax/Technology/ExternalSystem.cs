namespace P3Model.Parser.ModelSyntax.Technology;

public record ExternalSystem(string Name) : Element
{
    public record UsesProduct(ExternalSystem Source, Product Destination) 
        : Relation<ExternalSystem, Product>;
}
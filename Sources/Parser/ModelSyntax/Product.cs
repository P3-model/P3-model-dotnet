using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax;

public record Product(string Name) : Element
{
    public record UsesExternalSystem(Product Source, ExternalSystem Destination) 
        : Relation<Product, ExternalSystem>;
}
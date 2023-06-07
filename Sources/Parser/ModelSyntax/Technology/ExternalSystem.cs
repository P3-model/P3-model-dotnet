namespace P3Model.Parser.ModelSyntax.Technology;

public record ExternalSystem(string Name)
{
    public record UsesProduct(ExternalSystem ExternalSystem, Product Product);
}
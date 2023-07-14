namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record ModelBoundary(string Name) : Element
{
    public Perspective Perspective => Perspective.Domain;
}
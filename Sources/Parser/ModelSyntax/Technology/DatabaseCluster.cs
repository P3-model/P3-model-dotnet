namespace P3Model.Parser.ModelSyntax.Technology;

public record DatabaseCluster(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;
}
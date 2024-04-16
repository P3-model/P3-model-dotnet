namespace P3Model.Parser.ModelSyntax.Technology;

public class Layer(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Technology;
}
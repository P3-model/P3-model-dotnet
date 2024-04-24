namespace P3Model.Parser.ModelSyntax.Technology;

public class Layer(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.Technology;
}
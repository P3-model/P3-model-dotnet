namespace P3Model.Parser.ModelSyntax.Technology;

public class Layer : ElementBase
{
    public override Perspective Perspective => Perspective.Technology;

    public Layer(string name) : base(name) { }
    public Layer(string id, string name) : base(id, name) { }
}
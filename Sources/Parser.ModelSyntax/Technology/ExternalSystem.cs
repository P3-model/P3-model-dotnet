namespace P3Model.Parser.ModelSyntax.Technology;

public class ExternalSystem(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.Technology;
}
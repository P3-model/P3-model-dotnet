namespace P3Model.Parser.ModelSyntax.Technology;

public class ExternalSoftwareSystem : ElementBase
{
    public override Perspective Perspective => Perspective.Technology;

    public ExternalSoftwareSystem(string name) : base(name) { }
    public ExternalSoftwareSystem(string id, string name) : base(id, name) { }
}
namespace P3Model.Parser.ModelSyntax.Technology;

public record ExternalSoftwareSystem(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;    
    public string Id => Name;
}
namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public record CSharpType(HierarchyId Id, string Name, string Path) : CodeStructure
{
    public Perspective Perspective => Perspective.Technology;
    string Element.Id => Id.Full;
    
    public bool Equals(CodeStructure? other) => other is CSharpType otherType && Id.Equals(otherType.Id);
}
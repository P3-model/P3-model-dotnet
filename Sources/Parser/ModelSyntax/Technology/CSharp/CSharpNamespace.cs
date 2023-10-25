namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public record CSharpNamespace(HierarchyId Id, string Path) : CodeStructure
{
    public Perspective Perspective => Perspective.Technology;
    string Element.Id => Id.Full;
    public string Name => Id.LastPart;

    public bool Equals(CodeStructure? other) => other is CSharpNamespace otherNamespace && Id.Equals(otherNamespace.Id);
    
    public record ContainsNamespace(CSharpNamespace Source, CSharpNamespace Destination)
        : HierarchyRelation<CSharpNamespace>;

    public record ContainsType(CSharpNamespace Source, CSharpType Destination) : Relation<CSharpNamespace, CSharpType>;
}
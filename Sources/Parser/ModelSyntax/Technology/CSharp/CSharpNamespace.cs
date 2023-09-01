namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public record CSharpNamespace(string Name) : CodeStructure(Name)
{
    public record ContainsNamespace(CSharpNamespace Source, CSharpNamespace Destination)
        : HierarchyRelation<CSharpNamespace>;

    public record ContainsType(CSharpNamespace Source, CSharpType Destination) : Relation<CSharpNamespace, CSharpType>;
}
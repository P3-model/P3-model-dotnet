namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public record CSharpProject(string Name) : CodeStructure(Name)
{
    public record ReferencesProject(CSharpProject Source, CSharpProject Destination) 
        : HierarchyRelation<CSharpProject>;
    
    public record ContainsNamespace(CSharpProject Source, CSharpNamespace Destination) 
        : Relation<CSharpProject, CSharpNamespace>;
}
namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public record CSharpProject(string Name, string Path) : CodeStructure
{
    public Perspective Perspective => Perspective.Technology;
    public string Id => Name;
    
    public bool Equals(CodeStructure? other) => other is CSharpProject otherProject && Name.Equals(otherProject.Name);

    public record ReferencesProject(CSharpProject Source, CSharpProject Destination) 
        : HierarchyRelation<CSharpProject>;
    
    public record ContainsNamespace(CSharpProject Source, CSharpNamespace Destination) 
        : Relation<CSharpProject, CSharpNamespace>;
}
namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpProject(string name, string path) : ElementBase(name), CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    
    public string Path { get; } = path;

    public bool Equals(CodeStructure? other) => other != null && Equals((ElementBase)other);

    public record ReferencesProject(CSharpProject Source, CSharpProject Destination) 
        : HierarchyRelation<CSharpProject>;
    
    public record ContainsNamespace(CSharpProject Source, CSharpNamespace Destination) 
        : Relation<CSharpProject, CSharpNamespace>;
}
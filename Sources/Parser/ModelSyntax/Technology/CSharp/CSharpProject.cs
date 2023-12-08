namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpProject : ElementBase, CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    
    public string Path { get; }

    public CSharpProject(string name, string path) : base(name) => Path = path;
    public CSharpProject(string id, string name, string path) : base(id, name) => Path = path;

    public bool Equals(CodeStructure? other) => other != null && Equals((ElementBase)other);

    public record ReferencesProject(CSharpProject Source, CSharpProject Destination) 
        : HierarchyRelation<CSharpProject>;
    
    public record ContainsNamespace(CSharpProject Source, CSharpNamespace Destination) 
        : Relation<CSharpProject, CSharpNamespace>;
}
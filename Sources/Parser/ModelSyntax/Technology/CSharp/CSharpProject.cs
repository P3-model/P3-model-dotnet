namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpProject(string name, string path) : ElementBase(name), CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    
    public string Path { get; } = path;

    public bool Equals(CodeStructure? other) => other != null && Equals((ElementBase)other);

    public class ReferencesProject(CSharpProject source, CSharpProject destination) 
        : HierarchyRelation<CSharpProject>(source, destination);
    
    public class ContainsNamespace(CSharpProject source, CSharpNamespace destination) 
        : RelationBase<CSharpProject, CSharpNamespace>(source, destination);
}
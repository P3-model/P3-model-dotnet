namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public record CSharpProject(string Name) : CodeStructure(Name)
{
    public record ContainsNamespace(CSharpProject Source, CSharpNamespace Destination) 
        : Relation<CSharpProject, CSharpNamespace>;
}
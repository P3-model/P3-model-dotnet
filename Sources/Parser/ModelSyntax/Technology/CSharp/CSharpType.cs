namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpType : ElementBase, CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public string Path { get; }

    public CSharpType(string name, string path) : base(name) => Path = path;
    public CSharpType(string id, string name, string path) : base(id, name) => Path = path;

    public bool Equals(CodeStructure? other) => other != null && Equals(other);
}
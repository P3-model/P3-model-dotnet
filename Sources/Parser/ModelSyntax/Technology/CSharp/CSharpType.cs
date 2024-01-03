namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpType(string idPartUniqueForElementType, string name, string path)
    : ElementBase(idPartUniqueForElementType, name), CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public string Path { get; } = path;
}
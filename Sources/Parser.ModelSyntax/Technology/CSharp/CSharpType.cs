namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpType(ElementId id, string name, string sourceCodePath) : ElementBase(id, name), CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public string SourceCodePath { get; } = sourceCodePath;

    public override bool DataEquals(Element? other) =>
        base.DataEquals(other) &&
        other is CSharpType otherCSharpType &&
        SourceCodePath == otherCSharpType.SourceCodePath;
}
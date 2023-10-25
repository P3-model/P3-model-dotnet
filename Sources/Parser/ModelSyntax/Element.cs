namespace P3Model.Parser.ModelSyntax;

public interface Element
{
    Perspective Perspective { get; }
    string Id { get; }
    string Name { get; }
}
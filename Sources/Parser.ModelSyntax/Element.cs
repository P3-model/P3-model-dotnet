namespace P3Model.Parser.ModelSyntax;

public interface Element : IEquatable<Element>
{
    Perspective Perspective { get; }
    ElementId Id { get; }
    string Name { get; }
    
    bool DataEquals(Element? element);
}
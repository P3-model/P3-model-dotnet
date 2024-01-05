using Humanizer;

namespace P3Model.Parser.ModelSyntax;

public abstract class ElementBase : Element
{
    public abstract Perspective Perspective { get; }
    public string Id { get; }
    public string Name { get; }

    protected ElementBase(string name) : this(name.Dehumanize(), name) { }

    protected ElementBase(string idPartUniqueForElementType, string name)
    {
        Id = $"{GetType().Name}|{idPartUniqueForElementType}".ToLowerInvariant();
        Name = name;
    }

    public override bool Equals(object? obj) => obj is Element other && Equals(other);
    public bool Equals(Element? other) => Id == other?.Id;
    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"Type: {GetType().Name} | Name: {Name} | Id: {Id}";
}
using Humanizer;

namespace P3Model.Parser.ModelSyntax;

public abstract class ElementBase : Element
{
    public abstract Perspective Perspective { get; }
    public string Id { get; }
    public string Name { get; }

    protected ElementBase(string name)
    {
        Id = $"{GetType().Name}|{name.Dehumanize()}".ToLowerInvariant();
        Name = name;
    }
    
    protected ElementBase(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public override bool Equals(object? obj) => obj is Element other && Equals(other);
    public bool Equals(Element? other) => Id == other?.Id;
    public override int GetHashCode() => Id.GetHashCode();
}
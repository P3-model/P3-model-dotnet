using System.Runtime.CompilerServices;
using Humanizer;
using Serilog;

namespace P3Model.Parser.ModelSyntax;

public abstract class ElementBase : Element
{
    public abstract Perspective Perspective { get; }
    public ElementId Id { get; }
    public string Name { get; }

    protected ElementBase(ElementId id, string name)
    {
        Id = id;
        Name = name;
    }

    protected ElementBase(string name) : this(name.Dehumanize(), name) { }

    protected ElementBase(string idPartUniqueForElementType, string name)
    {
        Id = ElementId.Create(GetType(), idPartUniqueForElementType);
        Name = name;
    }

    protected static void SetOnce<T>(ref T? property, T value, ElementId elementId,
        [CallerMemberName] string? propertyName = null)
    {
        if (property is null)
            property = value;
        else if (!property.Equals(value))
            Log.Warning(
                $"Property: {propertyName} of element: {elementId.Value} has already been set. Existing value: {property}. New value: {value}. New value is ignored.");
    }

    public override bool Equals(object? obj) => obj is Element other && Equals(other);
    public bool Equals(Element? other) => Id == other?.Id;
    public override int GetHashCode() => Id.GetHashCode();

    public virtual bool DataEquals(Element? other) =>
        other is not null &&
        Id == other.Id &&
        Name == other.Name;

    public override string ToString() => $"Type: {GetType().Name} | Name: {Name} | Id: {Id}";

    public class DataEqualityComparer : IEqualityComparer<Element>
    {
        public bool Equals(Element? x, Element? y) => x != null && x.DataEquals(y);

        public int GetHashCode(Element obj) => obj.GetHashCode();
    }
}
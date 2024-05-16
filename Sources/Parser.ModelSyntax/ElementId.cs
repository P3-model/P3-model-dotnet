using JetBrains.Annotations;

namespace P3Model.Parser.ModelSyntax;

public readonly record struct ElementId(string Value) : IComparable<ElementId>
{
    [PublicAPI]
    public static ElementId Create<TElement>(string idPartUniqueForElementType)
        where TElement : Element =>
        Create(typeof(TElement), idPartUniqueForElementType);

    [PublicAPI]
    public static ElementId Create(Type type, string idPartUniqueForElementType) =>
        new($"{type.Name}|{idPartUniqueForElementType}".ToLowerInvariant());

    public int CompareTo(ElementId other) => string.Compare(Value, other.Value, StringComparison.Ordinal);

    public override string ToString() => Value;
}
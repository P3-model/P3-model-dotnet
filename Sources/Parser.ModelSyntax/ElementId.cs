namespace P3Model.Parser.ModelSyntax;

public readonly record struct ElementId(string Value)
{
    public static ElementId Create<TElement>(string idPartUniqueForElementType)
        where TElement : Element =>
        Create(typeof(TElement), idPartUniqueForElementType);

    public static ElementId Create(Type type, string idPartUniqueForElementType) =>
        new($"{type.Name}|{idPartUniqueForElementType}".ToLowerInvariant());
}
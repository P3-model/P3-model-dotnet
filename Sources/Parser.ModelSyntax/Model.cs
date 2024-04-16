using System.Collections.Immutable;

namespace P3Model.Parser.ModelSyntax;

public class Model(
    DocumentedSystem system,
    ImmutableArray<Element> elements,
    ImmutableArray<Relation> relations,
    ImmutableArray<Trait> traits)
{
    public DocumentedSystem System { get; } = system;
    public ImmutableArray<Element> Elements { get; } = elements;
    public ImmutableArray<Relation> Relations { get; } = relations;
    public ImmutableArray<Trait> Traits { get; } = traits;
}
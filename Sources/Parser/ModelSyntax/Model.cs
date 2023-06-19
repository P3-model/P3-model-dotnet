using System.Collections.Immutable;

namespace P3Model.Parser.ModelSyntax;

public class Model
{
    public ImmutableArray<Element> Elements { get; }
    public ImmutableArray<Relation> Relations { get; }
    public ImmutableArray<Trait> Traits { get; }
    
    public Model(ImmutableArray<Element> elements, ImmutableArray<Relation> relations, ImmutableArray<Trait> traits)
    {
        Elements = elements;
        Relations = relations;
        Traits = traits;
    }
}
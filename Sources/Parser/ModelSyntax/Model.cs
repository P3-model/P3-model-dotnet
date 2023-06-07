using System.Collections.Immutable;

namespace P3Model.Parser.ModelSyntax;

public class Model
{
    public ImmutableArray<Element> Elements { get; }
    public ImmutableArray<Relation> Relations { get; }
    public ModelCache Cache { get; }
    
    public Model(ImmutableArray<Element> elements, ImmutableArray<Relation> relations)
    {
        Elements = elements;
        Relations = relations;
        Cache = new ModelCache(this);
    }
}
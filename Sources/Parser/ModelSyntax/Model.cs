using System.Collections.Immutable;

namespace P3Model.Parser.ModelSyntax;

public class Model
{
    public DocumentedSystem System { get; }
    public ImmutableArray<Element> Elements { get; }
    public ImmutableArray<Relation> Relations { get; }
    public ImmutableArray<Trait> Traits { get; }
    
    public Model(DocumentedSystem system, ImmutableArray<Element> elements, ImmutableArray<Relation> relations, 
        ImmutableArray<Trait> traits)
    {
        System = system;
        Elements = elements;
        Relations = relations;
        Traits = traits;
    }
}
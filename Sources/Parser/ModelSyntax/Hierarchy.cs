using System.Collections.Generic;
using System.Linq;

namespace P3Model.Parser.ModelSyntax;

public class Hierarchy<TElement>
    where TElement : HierarchyElement
{
    private readonly Dictionary<TElement, List<TElement>> _parentToChildren;

    private Hierarchy(Dictionary<TElement, List<TElement>> parentToChildren) => _parentToChildren = parentToChildren;

    public static Hierarchy<TElement> Create<TRelation>(IEnumerable<TRelation> relations)
        where TRelation : HierarchyRelation<TElement> =>
        new(relations
            .GroupBy(r => r.Source, r => r.Destination,
                (parent, children) => (Parent: parent, Children: children))
            .ToDictionary(g => g.Parent, g => g.Children.ToList()));

    public IEnumerable<TElement> GetChildrenFor(TElement parent) => 
        _parentToChildren.TryGetValue(parent, out var children) 
            ? children 
            : Enumerable.Empty<TElement>();

    public IEnumerable<TElement> FromLevel(int level) => _parentToChildren.Keys
        .Where(module => module.Id.Level == level);
}
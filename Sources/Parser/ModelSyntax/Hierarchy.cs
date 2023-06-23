using System.Collections.Generic;
using System.Linq;

namespace P3Model.Parser.ModelSyntax;

public class Hierarchy<TElement>
    where TElement : HierarchyElement
{
    private readonly Dictionary<TElement, List<TElement>> _parentToChildren;
    private readonly Dictionary<TElement, TElement> _childToParent;

    private Hierarchy(Dictionary<TElement, List<TElement>> parentToChildren,
        Dictionary<TElement, TElement> childToParent)
    {
        _parentToChildren = parentToChildren;
        _childToParent = childToParent;
    }

    public static Hierarchy<TElement> Create<TRelation>(IEnumerable<TRelation> relations)
        where TRelation : HierarchyRelation<TElement>
    {
        var parentToChildren = new Dictionary<TElement, List<TElement>>();
        var childToParent = new Dictionary<TElement, TElement>();
        foreach (var relation in relations)
        {
            if (!parentToChildren.TryGetValue(relation.Source, out var children))
                parentToChildren.Add(relation.Source, children = new List<TElement>());
            children.Add(relation.Destination);
            childToParent.Add(relation.Destination, relation.Source);
        }
        return new(parentToChildren, childToParent);
    }

    public IReadOnlySet<TElement> GetAncestorsFor(TElement element, bool includeSelf = false)
    {
        var ancestors = new HashSet<TElement>();
        TElement? current;
        if (includeSelf)
            current = element;
        else if (!_childToParent.TryGetValue(element, out current))
            return ancestors;
        while (true)
        {
            ancestors.Add(current);
            if (!_childToParent.TryGetValue(current, out current))
                break;
        }
        return ancestors;
    }

    public IEnumerable<TElement> GetChildrenFor(TElement parent) =>
        _parentToChildren.TryGetValue(parent, out var children)
            ? children
            : Enumerable.Empty<TElement>();

    public IReadOnlySet<TElement> GetDescendantsFor(TElement element, bool includeSelf = false)
    {
        var descendants = GetChildrenFor(element)
            .SelectRecursively(GetChildrenFor)
            .ToHashSet();
        if (includeSelf)
            descendants.Add(element);
        return descendants;
    }

    public IEnumerable<TElement> FromLevel(int level) => _parentToChildren.Keys
        .Where(module => module.Id.Level == level);
}
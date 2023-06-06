using System;
using System.Collections.Generic;
using System.Linq;

namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public class DomainModulesHierarchy
{
    private readonly Dictionary<DomainModule, List<DomainModule>> _parentToChildren;

    public DomainModulesHierarchy(IEnumerable<DomainModule.ContainsDomainModule> relations) => _parentToChildren =
        relations
            .GroupBy(r => r.Parent, r => r.Child,
                (parent, children) => (Parent: parent, Children: children))
            .ToDictionary(g => g.Parent, g => g.Children.ToList());

    public IEnumerable<DomainModule> GetChildrenFor(DomainModule parent)
    {
        if (_parentToChildren.TryGetValue(parent, out var children))
            return children;
        return Array.Empty<DomainModule>();
    }

    public IEnumerable<DomainModule> FromLevel(int level) => _parentToChildren.Keys
        .Where(module => module.Level == level);
}
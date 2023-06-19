using System;
using System.Collections.Generic;
using System.Linq;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public class ProcessesHierarchy
{
    private readonly Dictionary<Process, List<Process>> _parentToChildren;

    public ProcessesHierarchy(IEnumerable<Process.ContainsSubProcess> relations) => _parentToChildren =
        relations
            .GroupBy(r => r.Source, r => r.Destination,
                (parent, children) => (Parent: parent, Children: children))
            .ToDictionary(g => g.Parent, g => g.Children.ToList());

    public IEnumerable<Process> GetChildrenFor(Process parent)
    {
        if (_parentToChildren.TryGetValue(parent, out var children))
            return children;
        return Array.Empty<Process>();
    }

    public IEnumerable<Process> FromFirstLevel()
    {
        var allChildren = new HashSet<Process>(_parentToChildren.Values.SelectMany(c => c));
        return _parentToChildren.Keys.Where(p => !allChildren.Contains(p));
    }
}
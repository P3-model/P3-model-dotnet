using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace P3Model.Parser.Configuration.Analyzers;

public class NamespaceOptionsBuilder
{
    private const char NamespacePartSeparator = '.';
    private readonly List<Func<string, string>> _namespaceFilters = [];

    [PublicAPI]
    public NamespaceOptionsBuilder RemoveRootNamespace(string rootNamespace) => Filter(currentHierarchy =>
        currentHierarchy.Equals(rootNamespace)
            ? string.Empty
            : currentHierarchy.StartsWith(rootNamespace)
                ? currentHierarchy[(rootNamespace.Length + 1)..]
                : currentHierarchy);

    // TODO: Extract namespaceFilters to separate types to test them without ISymbol
    [PublicAPI]
    public NamespaceOptionsBuilder RemoveNamespacePart(params string[] partsToRemove) => Filter(initialHierarchy =>
        partsToRemove.Aggregate(initialHierarchy,
            (currentHierarchy, partToRemove) =>
            {
                if (currentHierarchy.Equals(partToRemove))
                    return string.Empty;
                var partToRemoveStartIndex = currentHierarchy.IndexOf(partToRemove, StringComparison.InvariantCulture);
                if (partToRemoveStartIndex == -1)
                    return currentHierarchy;
                var prevNamespacePartSeparatorIndex = currentHierarchy.LastIndexOf(NamespacePartSeparator,
                    partToRemoveStartIndex);
                var namespacePartStartIndex = prevNamespacePartSeparatorIndex == -1
                    ? 0
                    : prevNamespacePartSeparatorIndex + 1;
                if (partToRemoveStartIndex != namespacePartStartIndex)
                    return currentHierarchy;
                var partToRemoveEndIndex = partToRemoveStartIndex + partToRemove.Length - 1;
                var nextNamespacePartSeparatorIndex = currentHierarchy.IndexOf(NamespacePartSeparator,
                    partToRemoveStartIndex);
                var namespacePartEndIndex = nextNamespacePartSeparatorIndex == -1
                    ? currentHierarchy.Length - 1
                    : nextNamespacePartSeparatorIndex - 1;
                if (partToRemoveEndIndex != namespacePartEndIndex)
                    return currentHierarchy;
                return partToRemoveStartIndex == 0
                    ? currentHierarchy[(partToRemove.Length + 1)..]
                    : currentHierarchy.Remove(partToRemoveStartIndex - 1, partToRemove.Length + 1);
            }));

    private NamespaceOptionsBuilder Filter(Func<string, string> filter)
    {
        _namespaceFilters.Add(filter);
        return this;
    }

    public NamespaceOptions Build() => new(
        symbol => _namespaceFilters.Aggregate(
            symbol.ToDisplayString(),
            (currentHierarchy, filter) => filter(currentHierarchy)));
}
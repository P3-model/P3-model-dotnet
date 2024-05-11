using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace P3Model.Parser.CodeAnalysis.Configuration.Analyzers;

public class NamespaceOptionsBuilder
{
    private const char NamespacePartSeparator = '.';
    private readonly List<string> _namespacePartsToSkip = [];

    [PublicAPI]
    public NamespaceOptionsBuilder SkipNamespacePart(params string[] partsToSkip)
    {
        _namespacePartsToSkip.AddRange(partsToSkip.SelectMany(part => part.Split(NamespacePartSeparator)));
        return this;
    }

    public NamespaceOptions Build() => new([.._namespacePartsToSkip]);
}
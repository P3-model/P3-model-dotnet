using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.ModelSyntax;

public interface ElementsProvider : IEnumerable<ElementInfo>
{
    [PublicAPI]
    IEnumerable<Element> For(ISymbol symbol);

    [PublicAPI]
    IEnumerable<TElement> OfType<TElement>() where TElement : Element;
}
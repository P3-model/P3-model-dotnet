using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

public interface ElementsProvider : IEnumerable<ElementInfo>
{
    [PublicAPI]
    IEnumerable<ElementBase> For(ISymbol symbol);

    [PublicAPI]
    IEnumerable<TElement> OfType<TElement>() where TElement : ElementBase;
}
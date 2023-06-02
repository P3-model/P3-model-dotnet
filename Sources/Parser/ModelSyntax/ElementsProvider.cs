using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.ModelSyntax;

public interface ElementsProvider
{
    [PublicAPI]
    IEnumerable<Element> For(ISymbol symbol);

    [PublicAPI]
    IEnumerable<Element> Where(Func<ISymbol, bool> predicate);
}
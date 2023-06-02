using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis;

public static class SymbolExtensions
{
    // TODO: support for attributes from different versions of annotation assembly
    public static bool TryGetAttribute(this ISymbol symbol, Type type,
        [NotNullWhen(true)] out AttributeData? attributeData)
    {
        attributeData = symbol
            .GetAttributes()
            .SingleOrDefault(attributeData =>
                attributeData.AttributeClass?.Name == type.Name &&
                attributeData.AttributeClass?.ContainingNamespace.ToDisplayString() == type.Namespace);
        return attributeData != null;
    }

    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, Type type) => symbol
        .GetAttributes()
        .Where(attributeData =>
            attributeData.AttributeClass?.Name == type.Name &&
            attributeData.AttributeClass?.ContainingNamespace.ToDisplayString() == type.Namespace);
}
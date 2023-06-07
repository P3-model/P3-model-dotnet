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

    public static T GetConstructorParameterValue<T>(this AttributeData attributeData) =>
        (T)attributeData.ConstructorArguments[0].Value!;
    
    public static bool TryGetConstructorParameterValue<T>(this AttributeData attributeData, 
        [NotNullWhen(true)] out T? value)
    {
        var valueAsObject = attributeData.ConstructorArguments[0].Value;
        if (valueAsObject is null)
        {
            value = default;
            return false;
        }

        value = (T)valueAsObject;
        return true;
    }
}
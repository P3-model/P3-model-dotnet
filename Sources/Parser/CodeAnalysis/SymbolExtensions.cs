using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis;

// TODO: missing vs. null argument value
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

    public static T GetConstructorArgumentValue<T>(this AttributeData attributeData)
    {
        if (attributeData.ConstructorArguments.Length != 1)
            throw new InvalidOperationException();
        return (T)attributeData.ConstructorArguments[0].Value!;
    }

    public static T GetConstructorArgumentValue<T>(this AttributeData attributeData, string argumentName)
    {
        if (!TryGetConstructorArgumentValue<T>(attributeData, argumentName, out var value))
            throw new InvalidOperationException();
        return value;
    }

    public static bool TryGetConstructorArgumentValue<T>(this AttributeData attributeData, string argumentName,
        [NotNullWhen(true)] out T? value)
    {
        var argumentIndex = attributeData.AttributeConstructor
            ?.Parameters
            .SingleOrDefault(symbol => symbol.Name.Equals(argumentName, StringComparison.InvariantCultureIgnoreCase))
            ?.Ordinal;
        if (argumentIndex is null)
        {
            value = default;
            return false;
        }

        value = (T)attributeData.ConstructorArguments[argumentIndex.Value].Value!;
        return true;
    }

    public static ImmutableArray<T> GetConstructorArgumentValues<T>(this AttributeData attributeData,
        string argumentName)
    {
        if (!TryGetConstructorArgumentValues<T>(attributeData, argumentName, out var values))
            throw new InvalidOperationException();
        return values.Value;
    }

    public static bool TryGetConstructorArgumentValues<T>(this AttributeData attributeData, string argumentName,
        [NotNullWhen(true)] out ImmutableArray<T>? values)
    {
        var argumentIndex = attributeData.AttributeConstructor
            ?.Parameters
            .SingleOrDefault(symbol => symbol.Name.Equals(argumentName, StringComparison.InvariantCultureIgnoreCase))
            ?.Ordinal;
        if (argumentIndex is null)
        {
            values = default;
            return false;
        }

        values = attributeData.ConstructorArguments[argumentIndex.Value]
            .Values
            .Select(v => v.Value)
            .Cast<T>()
            .ToImmutableArray();
        return true;
    }

    public static bool TryGetNamedArgumentValue<T>(this AttributeData attributeData, string argumentName,
        [NotNullWhen(true)] out T? value)
    {
        var pair = attributeData.NamedArguments
            .SingleOrDefault(p => p.Key.Equals(argumentName, StringComparison.InvariantCultureIgnoreCase));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        // Can be null because of SingleOrDefault invocation.
        if (pair.Key is null)
        {
            value = default;
            return false;
        }

        value = (T)pair.Value.Value!;
        return true;
    }
    
    public static bool TryGetNamedArgumentValues<T>(this AttributeData attributeData, string argumentName,
        [NotNullWhen(true)] out ImmutableArray<T>? values)
    {
        var pair = attributeData.NamedArguments
            .SingleOrDefault(p => p.Key.Equals(argumentName, StringComparison.InvariantCultureIgnoreCase));
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        // Can be null because of SingleOrDefault invocation.
        if (pair.Key is null)
        {
            values = default;
            return false;
        }

        values = pair.Value
            .Values
            .Select(v => v.Value)
            .Cast<T>()
            .ToImmutableArray();
        return true;
    }
}
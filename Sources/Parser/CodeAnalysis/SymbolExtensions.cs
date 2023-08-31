using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis;

[Flags]
public enum GetAttributeOptions
{
    Default = 0,
    IncludeAttributeBaseTypes = 1
}

// TODO: missing vs. null argument value
public static class SymbolExtensions
{
    // TODO: support for attributes from different versions of annotation assembly
    public static bool TryGetAttribute(this ISymbol symbol, Type type,
        [NotNullWhen(true)] out AttributeData? attributeData) =>
        TryGetAttribute(symbol, type, GetAttributeOptions.Default, out attributeData);

    public static bool TryGetAttribute(this ISymbol symbol, Type type, GetAttributeOptions options,
        [NotNullWhen(true)] out AttributeData? attributeData)
    {
        if (options.HasFlag(GetAttributeOptions.IncludeAttributeBaseTypes))
        {
            attributeData = symbol
                .GetAttributes()
                .SingleOrDefault(a => a.AttributeClass.IsAssignableFrom(type));
            return attributeData != null;
        }
        attributeData = symbol
            .GetAttributes()
            .SingleOrDefault(a => a.AttributeClass.IsExactlyOf(type));
        return attributeData != null;
    }

    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, Type type) =>
        GetAttributes(symbol, type, GetAttributeOptions.Default);

    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, Type type, GetAttributeOptions options)
    {
        if (options.HasFlag(GetAttributeOptions.IncludeAttributeBaseTypes))
        {
            return symbol
                .GetAttributes()
                .Where(attribute => attribute.AttributeClass.IsAssignableFrom(type));
        }
        return symbol
            .GetAttributes()
            .Where(attribute => attribute.AttributeClass.IsExactlyOf(type));
    }

    private static bool IsAssignableFrom(this INamedTypeSymbol? attributeSymbol, Type type)
    {
        while (attributeSymbol != null)
        {
            if (attributeSymbol.IsExactlyOf(type))
                return true;
            attributeSymbol = attributeSymbol.BaseType;
        }
        return false;
    }

    private static bool IsExactlyOf(this INamedTypeSymbol? attributeSymbol, Type type) =>
        attributeSymbol != null &&
        attributeSymbol.Name == type.Name &&
        attributeSymbol.ContainingNamespace.ToDisplayString() == type.Namespace;

    public static T GetArgumentValue<T>(this AttributeData attributeData, string argumentName)
    {
        if (!TryGetConstructorArgumentValue(attributeData, argumentName, out T? value) &&
            !TryGetNamedArgumentValue(attributeData, argumentName, out value))
            throw new InvalidOperationException();
        return value;
    }

    public static bool TryGetArgumentValue<T>(this AttributeData attributeData, string argumentName,
        [NotNullWhen(true)] out T? value) =>
        TryGetConstructorArgumentValue(attributeData, argumentName, out value) ||
        TryGetNamedArgumentValue(attributeData, argumentName, out value);

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

    public static IEnumerable<T> GetConstructorArgumentValues<T>(this AttributeData attributeData,
        string argumentName)
    {
        if (!TryGetConstructorArgumentValues<T>(attributeData, argumentName, out var values))
            throw new InvalidOperationException();
        return values;
    }

    public static bool TryGetConstructorArgumentValues<T>(this AttributeData attributeData, string argumentName,
        [NotNullWhen(true)] out IEnumerable<T>? values)
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
            .Cast<T>();
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
        [NotNullWhen(true)] out IEnumerable<T>? values)
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
            .Cast<T>();
        return true;
    }

    public static string GetFullName(this ISymbol symbol) => symbol switch
    {
        INamedTypeSymbol typeSymbol => GetFullName(typeSymbol),
        _ => symbol.Name
    };

    public static string GetFullName(this INamedTypeSymbol symbol) =>
        symbol.ContainingSymbol is INamedTypeSymbol containingSymbol
            ? $"{containingSymbol.GetFullName()} {symbol.Name}"
            : symbol.Name;

    // TODO: better way to select assemblies from analyzed solution
    public static IEnumerable<IAssemblySymbol> GetReferencedAssembliesFromSameRepository(this IAssemblySymbol symbol) =>
        symbol.Modules
            .SelectMany(m => m.ReferencedAssemblySymbols)
            .Where(a => a.Locations
                .Any(l => l.IsInSource));

    public static bool ContainsSourcesOf(this DirectoryInfo directory, ISymbol symbol) =>
        SourcesAreIn(symbol, directory);

    public static bool SourcesAreIn(this ISymbol symbol, DirectoryInfo directory) => symbol.Locations
        .Any(location => location.SourceTree?.FilePath.StartsWith(directory.FullName) ?? false);

    public static bool SourcesAreInAny(this ISymbol symbol, IEnumerable<DirectoryInfo> directories) => symbol.Locations
        .Any(location => directories
            .Any(directory => location.SourceTree?.FilePath.StartsWith(directory.FullName) ?? false));

    public static bool IsFrom(this ISymbol symbol, IAssemblySymbol assemblySymbol) => symbol switch
    {
        INamespaceSymbol namespaceSymbol => namespaceSymbol.ConstituentNamespaces
            .Any(n => n.ContainingAssembly.Equals(assemblySymbol, SymbolEqualityComparer.Default)),
        _ => symbol.ContainingAssembly.Equals(assemblySymbol, SymbolEqualityComparer.Default)
    };
}
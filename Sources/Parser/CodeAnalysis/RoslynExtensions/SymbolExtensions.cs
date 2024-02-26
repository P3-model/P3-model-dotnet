using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using P3Model.Annotations;
using P3Model.Parser.ModelSyntax;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

// TODO: missing vs. null argument value
public static class SymbolExtensions
{
    // TODO: support for attributes from different versions of annotation assembly
    public static bool TryGetAttribute(this ISymbol symbol,
        AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData) => symbol switch
    {
        ITypeSymbol typeSymbol => TryGetAttribute(typeSymbol, selector, out attributeData),
        INamespaceSymbol namespaceSymbol => TryGetAttribute(namespaceSymbol, selector, out attributeData),
        IAssemblySymbol assemblySymbol => TryGetAttribute(assemblySymbol, selector, out attributeData),
        IMethodSymbol methodSymbol => TryGetAttribute(methodSymbol, selector, out attributeData),
        _ => throw new ParserError($"Getting attribute for symbol {symbol.GetType().Name} is not supported")
    };

    public static bool TryGetAttribute(this ITypeSymbol symbol,
        AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData) =>
        TryGetAttribute(symbol, selector, TypeAttributeSources.Self, out attributeData, out _);

    public static bool TryGetAttribute(this ITypeSymbol symbol,
        AttributeSelector selector,
        TypeAttributeSources sources,
        [NotNullWhen(true)] out AttributeData? attributeData,
        [NotNullWhen(true)] out ITypeSymbol? annotatedSymbol)
    {
        if (TryGetDirectAttribute(symbol, selector, out attributeData))
        {
            annotatedSymbol = symbol;
            return true;
        }
        if (sources.HasFlag(TypeAttributeSources.BaseClasses) &&
            TryGetBaseClassAttribute(symbol, selector, sources, out attributeData, out annotatedSymbol))
            return true;
        if (sources.HasFlag(TypeAttributeSources.AllInterfaces) &&
            TryGetInterfaceAttribute(symbol, selector, sources, out attributeData, out annotatedSymbol))
            return true;
        annotatedSymbol = null;
        return false;
    }

    public static bool TryGetAttribute(this INamespaceSymbol symbol,
        AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData)
    {
        if (!typeof(NamespaceApplicable).IsAssignableFrom(selector.Type))
        {
            Log.Warning(
                "Not namespace applicable attribute {attributeType} used to get attribute from namespace symbol",
                selector.Type.FullName);
            attributeData = null;
            return false;
        }
        foreach (var typeSymbol in symbol.GetTypeMembers())
        {
            if (typeSymbol.TryGetAttribute(selector, out attributeData) &&
                attributeData.TryGetNamedArgumentValue<bool>(nameof(NamespaceApplicable.ApplyOnNamespace),
                    out var applyOnNamespace) &&
                applyOnNamespace)
                return true;
        }
        attributeData = null;
        return false;
    }

    public static bool TryGetAttribute(this IAssemblySymbol symbol,
        AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData) =>
        TryGetDirectAttribute(symbol, selector, out attributeData);

    public static bool TryGetAttribute(this IMethodSymbol symbol, AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData) =>
        TryGetDirectAttribute(symbol, selector, out attributeData);

    public static bool TryGetAssemblyAttribute(this ISymbol symbol, AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData)
    {
        if (symbol is INamespaceSymbol namespaceSymbol)
            return namespaceSymbol.TryGetAssemblyAttribute(selector, out attributeData);
        return symbol.ContainingAssembly.TryGetAttribute(selector, out attributeData);
    }

    public static bool TryGetAssemblyAttribute(this INamespaceSymbol namespaceSymbol,
        AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData)
    {
        foreach (var constituentSymbol in namespaceSymbol.ConstituentNamespaces)
        {
            if (constituentSymbol.ContainingAssembly.TryGetAttribute(selector, out attributeData))
                return true;
        }
        attributeData = null;
        return false;
    }

    private static bool TryGetDirectAttribute(ISymbol symbol,
        AttributeSelector selector,
        [NotNullWhen(true)] out AttributeData? attributeData)
    {
        Func<AttributeData, bool> predicate = selector.IncludeBaseTypes
            ? a => a.AttributeClass.IsAssignableFrom(selector.Type)
            : a => a.AttributeClass.IsExactlyOf(selector.Type);
        attributeData = symbol.GetAttributes().SingleOrDefault(predicate);
        return attributeData != null;
    }

    private static bool TryGetBaseClassAttribute(ITypeSymbol typeSymbol,
        AttributeSelector selector,
        TypeAttributeSources sources,
        [NotNullWhen(true)] out AttributeData? attributeData,
        [NotNullWhen(true)] out ITypeSymbol? annotatedSymbol)
    {
        sources = sources.Without(TypeAttributeSources.AllInterfaces);
        if (typeSymbol.BaseType != null &&
            typeSymbol.BaseType.TryGetAttribute(selector, sources, out attributeData, out annotatedSymbol))
            return true;
        attributeData = null;
        annotatedSymbol = null;
        return false;
    }

    private static bool TryGetInterfaceAttribute(ITypeSymbol typeSymbol,
        AttributeSelector selector,
        TypeAttributeSources sources,
        [NotNullWhen(true)] out AttributeData? attributeData, 
        [NotNullWhen(true)] out ITypeSymbol? annotatedSymbol)
    {
        sources = sources
            .Without(TypeAttributeSources.BaseClasses)
            .Without(TypeAttributeSources.AllInterfaces);
        foreach (var interfaceSymbol in typeSymbol.AllInterfaces)
        {
            if (interfaceSymbol.TryGetAttribute(selector, sources, out attributeData, out annotatedSymbol))
                return true;
        }
        attributeData = null;
        annotatedSymbol = null;
        return false;
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
            .Any(n => n.ContainingAssembly.CompilationIndependentEquals(assemblySymbol)),
        _ => symbol.ContainingAssembly.CompilationIndependentEquals(assemblySymbol)
    };

    public static IEnumerable<string> GetSourceCodePaths(this ISymbol symbol) => symbol
        .Locations
        .Select(location => location.SourceTree?.FilePath)
        .Where(path => path != null)!;

    private static bool CompilationIndependentEquals(this ISymbol symbol, ISymbol other) =>
        CompilationIndependentSymbolEqualityComparer.Default.Equals(symbol, other);
}
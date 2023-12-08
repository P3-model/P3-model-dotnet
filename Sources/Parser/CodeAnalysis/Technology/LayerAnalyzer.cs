using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Annotations.Technology.CleanArchitecture;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class LayerAnalyzer : SymbolAnalyzer<IAssemblySymbol>, SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (!IsLayer(symbol, out var name))
            return;
        name = name.Humanize();
        var layer = new Layer(name);
        modelBuilder.Add(layer, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, layer, elements));
    }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(LayerAttribute), GetAttributeOptions.IncludeAttributeBaseTypes,
                out var layerAttribute))
            return;
        var name = layerAttribute.GetConstructorArgumentValue<string>(nameof(LayerAttribute.Name)).Humanize();
        var layer = new Layer(name);
        ISymbol targetSymbol = layerAttribute.TryGetNamedArgumentValue<bool>(nameof(LayerAttribute.ApplyOnNamespace),
            out var applyOnNamespace) && applyOnNamespace
            ? symbol.ContainingNamespace
            : symbol;
        modelBuilder.Add(layer, targetSymbol);
        modelBuilder.Add(elements => GetRelations(targetSymbol, layer, elements));
    }

    private static bool IsLayer(ISymbol symbol, [NotNullWhen(true)] out string? name)
    {
        // TODO: better way to get value passed to base constructor
        if (symbol.TryGetAttribute(typeof(LayerAttribute), out var layerAttribute))
        {
            name = layerAttribute.GetConstructorArgumentValue<string>(nameof(LayerAttribute.Name));
            return true;
        }
        if (symbol.TryGetAttribute(typeof(EntitiesLayerAttribute), out layerAttribute))
        {
            name = EntitiesLayerAttribute.Name;
            return true;
        }
        if (symbol.TryGetAttribute(typeof(UseCasesLayerAttribute), out layerAttribute))
        {
            name = UseCasesLayerAttribute.Name;
            return true;
        }
        if (symbol.TryGetAttribute(typeof(AdaptersLayerAttribute), out layerAttribute))
        {
            name = AdaptersLayerAttribute.Name;
            return true;
        }
        if (symbol.TryGetAttribute(typeof(FrameworkLayerAttribute), out layerAttribute))
        {
            name = FrameworkLayerAttribute.Name;
            return true;
        }

        name = null;
        return false;
    }

    private static IEnumerable<Relation> GetRelations(ISymbol symbol, Layer layer, ElementsProvider elements) =>
        elements.For(symbol)
            .OfType<CodeStructure>()
            .Select(codeStructure => new CodeStructure.BelongsToLayer(codeStructure, layer));
}
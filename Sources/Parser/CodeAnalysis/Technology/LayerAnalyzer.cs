using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class LayerAnalyzer : SymbolAnalyzer<IAssemblySymbol>, SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(LayerAttribute), GetAttributeOptions.IncludeAttributeBaseTypes,
                out var layerAttribute))
            return;
        var name = layerAttribute.GetConstructorArgumentValue<string>(nameof(LayerAttribute.Name));
        var layer = new Layer(name);
        modelBuilder.Add(layer, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, layer, elements));
    }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(LayerAttribute), GetAttributeOptions.IncludeAttributeBaseTypes,
                out var layerAttribute))
            return;
        var name = layerAttribute.GetConstructorArgumentValue<string>(nameof(LayerAttribute.Name));
        var layer = new Layer(name);
        ISymbol targetSymbol = layerAttribute.TryGetNamedArgumentValue<bool>(nameof(LayerAttribute.ApplyOnNamespace),
            out var applyOnNamespace) && applyOnNamespace
            ? symbol.ContainingNamespace
            : symbol;
        modelBuilder.Add(layer, targetSymbol);
        modelBuilder.Add(elements => GetRelations(targetSymbol, layer, elements));
    }

    private static IEnumerable<Relation> GetRelations(ISymbol symbol, Layer layer, ElementsProvider elements) =>
        elements.For(symbol)
            .OfType<CodeStructure>()
            .Select(codeStructure => new CodeStructure.BelongsToLayer(codeStructure, layer));
}
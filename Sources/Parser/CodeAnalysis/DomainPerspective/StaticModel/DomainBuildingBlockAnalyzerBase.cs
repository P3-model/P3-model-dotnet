using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

public abstract class DomainBuildingBlockAnalyzerBase : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
{
    protected abstract Type AttributeType { get; }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) =>  Analyze((ISymbol)symbol, modelBuilder);

    private void Analyze(ISymbol symbol, ModelBuilder modelBuilder)
    {
        // TODO: Support for duplicated symbols (partial classes)
        if (!symbol.TryGetAttribute(AttributeType, out var buildingBlockAttribute))
            return;
        var name = GetName(symbol, buildingBlockAttribute);
        var descriptionFile = GetDescriptionFile(symbol);
        var buildingBlock = CreateBuildingBlock(name, descriptionFile);
        modelBuilder.Add(buildingBlock, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, buildingBlock, buildingBlockAttribute, elements));
    }

    private static string GetName(ISymbol symbol, AttributeData buildingBlockAttribute) =>
        buildingBlockAttribute.GetConstructorArgumentValue<string?>(nameof(DomainBuildingBlockAttribute.Name))
        ?? symbol.GetFullName().Humanize(LetterCasing.Title);

    private static FileInfo? GetDescriptionFile(ISymbol symbol) => symbol.Locations
        .Where(location => location.SourceTree != null)
        .Select(location =>
        {
            var directory = Path.GetDirectoryName(location.SourceTree!.FilePath);
            if (string.IsNullOrEmpty(directory))
                throw new InvalidOperationException();
            var fileName = Path.GetFileNameWithoutExtension(location.SourceTree!.FilePath);
            var path = Path.Combine(directory, $"{fileName}.md");
            return new FileInfo(path);
        })
        .SingleOrDefault(file => file.Exists);

    protected abstract DomainBuildingBlock CreateBuildingBlock(string name, FileInfo? descriptionFile);

    protected virtual IEnumerable<Relation> GetRelations(ISymbol symbol, DomainBuildingBlock buildingBlock, 
        AttributeData buildingBlockAttribute, ElementsProvider elements)
    {
        var module = elements.For(symbol.ContainingNamespace)
            .OfType<DomainModule>()
            .SingleOrDefault();
        if (module != null)
            yield return new DomainModule.ContainsBuildingBlock(module, buildingBlock);
    }
}
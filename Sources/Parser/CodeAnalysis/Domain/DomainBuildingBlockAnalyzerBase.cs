using System;
using System.IO;
using System.Linq;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Annotations;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.Domain;

public abstract class DomainBuildingBlockAnalyzerBase<TBuildingBlock>(
    DomainModulesHierarchyResolver modulesHierarchyResolver)
    : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
    where TBuildingBlock : DomainBuildingBlock
{
    protected abstract Type AttributeType { get; }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        const TypeAttributeSources sources = TypeAttributeSources.Self |
            TypeAttributeSources.BaseClasses |
            TypeAttributeSources.AllInterfaces;
        if (!symbol.TryGetAttribute(AttributeType, sources, out var buildingBlockAttribute, out var annotatedSymbol))
            return;
        if (IsAnnotatedDirectly(symbol, annotatedSymbol))
        {
            if (symbol.IsExplicitlyExcludedFromDomainModel())
                LogSymbolExcludedFromDomainModel(symbol);
        }
        else if (!symbol.IsExplicitlyIncludedInDomainModel())
        {
            return;
        }
        // TODO: Support for duplicated symbols (partial classes)
        var (buildingBlock, module) = GetBuildingBlock(symbol, buildingBlockAttribute);
        SetProperties(buildingBlock, symbol);
        AddElementsAndRelations(buildingBlock, module, symbol, buildingBlockAttribute, modelBuilder);
    }

    private static bool IsAnnotatedDirectly(ISymbol analyzedSymbol, ISymbol annotatedSymbol) =>
        CompilationIndependentSymbolEqualityComparer.Default.Equals(analyzedSymbol, annotatedSymbol);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        if (!symbol.TryGetAttribute(AttributeType, out var buildingBlockAttribute))
            return;
        if (symbol.IsExplicitlyExcludedFromDomainModel())
            LogSymbolExcludedFromDomainModel(symbol);
        // TODO: Support for duplicated symbols (partial classes)
        var (buildingBlock, module) = GetBuildingBlock(symbol, buildingBlockAttribute);
        SetProperties(buildingBlock, symbol);
        AddElementsAndRelations(buildingBlock, module, symbol, buildingBlockAttribute, modelBuilder);
    }

    private (TBuildingBlock, DomainModule?) GetBuildingBlock(ISymbol symbol,
        AttributeData buildingBlockAttribute)
    {
        var name = GetName(symbol, buildingBlockAttribute);
        if (modulesHierarchyResolver.TryFind(symbol, out var hierarchyId))
        {
            var buildingBlock = CreateBuildingBlock($"{hierarchyId.Value.Full}.{name.Dehumanize()}", name);
            var module = new DomainModule(
                ElementId.Create<DomainModule>(hierarchyId.Value.Full),
                hierarchyId.Value);
            return (buildingBlock, module);
        }
        else
        {
            var buildingBlock = CreateBuildingBlock(name.Dehumanize(), name);
            return (buildingBlock, null);
        }
    }

    private static string GetName(ISymbol symbol, AttributeData buildingBlockAttribute)
    {
        if (!buildingBlockAttribute.TryGetConstructorArgumentValue<string?>(
                nameof(DomainBuildingBlockAttribute.Name),
                out var name) ||
            string.IsNullOrWhiteSpace(name))
            name = symbol.GetFullName();
        return name.Humanize(LetterCasing.Title);
    }

    protected abstract TBuildingBlock CreateBuildingBlock(string idPartUniqueForElementType, string name);

    private static string? GetShortDescription(ISymbol symbol) =>
        symbol.TryGetAttribute(typeof(ShortDescriptionAttribute), out var descriptionAttribute)
            ? descriptionAttribute.GetConstructorArgumentValue<string>(nameof(ShortDescriptionAttribute.MarkdownText))
            : null;

    private static void SetProperties(DomainBuildingBlock buildingBlock, ISymbol symbol)
    {
        var shortDescription = GetShortDescription(symbol);
        buildingBlock.ShortDescription = shortDescription;
    }

    protected virtual void AddElementsAndRelations(TBuildingBlock buildingBlock, DomainModule? module,
        ISymbol symbol, AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        modelBuilder.Add(buildingBlock, symbol);
        if (module != null)
        {
            modelBuilder.Add(module, symbol);
            modelBuilder.Add(new DomainModule.ContainsBuildingBlock(module, buildingBlock));
        }
        modelBuilder.Add(elements => elements
            .For(symbol)
            .OfType<CodeStructure>()
            .Select(codeStructure =>
                new DomainBuildingBlock.IsImplementedBy(buildingBlock, codeStructure)));
    }

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

    private void LogSymbolExcludedFromDomainModel(ISymbol symbol) =>
        Log.Warning(
            "Symbol: {symbol} associated with Building Block: {buildingBlock} is explicitly excluded from domain model",
            symbol.ToDisplayString(),
            AttributeType.Name.Replace("Attribute", string.Empty));
}
using System;
using System.IO;
using System.Linq;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Annotations;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public abstract class DomainBuildingBlockAnalyzerBase<TBuildingBlock>(DomainModuleFinder moduleFinder)
    : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
    where TBuildingBlock : DomainBuildingBlock
{
    protected abstract Type AttributeType { get; }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze(symbol,
        TypeAttributeSources.Self | TypeAttributeSources.BaseClasses | TypeAttributeSources.AllInterfaces,
        modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) =>
        Analyze(symbol, TypeAttributeSources.Self, modelBuilder);

    private void Analyze(ISymbol symbol, TypeAttributeSources sources, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        if (!symbol.BelongsToDomainModel(AttributeType, sources, out var buildingBlockAttribute))
            return;
        // TODO: Support for duplicated symbols (partial classes)
        var (buildingBlock, module) = GetBuildingBlock(symbol, buildingBlockAttribute);
        SetProperties(buildingBlock, symbol);
        AddElementsAndRelations(buildingBlock, module, symbol, buildingBlockAttribute, modelBuilder);
    }

    private (TBuildingBlock, DomainModule?) GetBuildingBlock(ISymbol symbol,
        AttributeData buildingBlockAttribute)
    {
        var name = GetName(symbol, buildingBlockAttribute);
        var id = moduleFinder.TryFind(symbol, out var module)
            ? $"{module.Id.Full}.{name.Dehumanize()}"
            : name.Dehumanize();
        var buildingBlock = CreateBuildingBlock(id, name);
        return (buildingBlock, module);
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

    protected abstract TBuildingBlock CreateBuildingBlock(string id, string name);

    private static string? GetShortDescription(ISymbol symbol) =>
        symbol.TryGetAttribute(typeof(ShortDescriptionAttribute), out var descriptionAttribute)
            ? descriptionAttribute.GetConstructorArgumentValue<string>(nameof(ShortDescriptionAttribute.MarkdownText))
            : null;

    private static void SetProperties(DomainBuildingBlock buildingBlock, ISymbol symbol)
    {
        var shortDescription = GetShortDescription(symbol);
        buildingBlock.ShortDescription = shortDescription;
    }

    protected virtual void AddElementsAndRelations(TBuildingBlock externalSystemIntegration, DomainModule? module,
        ISymbol symbol, AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        modelBuilder.Add(externalSystemIntegration, symbol);
        if (module != null)
        {
            modelBuilder.Add(module, symbol);
            modelBuilder.Add(new DomainModule.ContainsBuildingBlock(module, externalSystemIntegration));
        }
        modelBuilder.Add(elements => elements
            .For(symbol)
            .OfType<CodeStructure>()
            .Select(codeStructure =>
                new DomainBuildingBlock.IsImplementedBy(externalSystemIntegration, codeStructure)));
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
}
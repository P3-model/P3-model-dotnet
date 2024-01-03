using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public abstract class DomainBuildingBlockAnalyzerBase : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
{
    private readonly DomainModuleFinder _moduleFinder;

    protected DomainBuildingBlockAnalyzerBase(DomainModuleFinder moduleFinder) =>
        _moduleFinder = moduleFinder;

    protected abstract Type AttributeType { get; }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    private void Analyze(ISymbol symbol, ModelBuilder modelBuilder)
    {
        // TODO: Support for duplicated symbols (partial classes)
        if (!symbol.TryGetAttribute(AttributeType, out var buildingBlockAttribute))
            return;
        var hasModule = _moduleFinder.TryFind(symbol, out var module);
        var name = GetName(symbol, buildingBlockAttribute);
        var id = module is null ? name.Dehumanize() : $"{module.Id.Full}.{name.Dehumanize()}";
        var buildingBlock = CreateBuildingBlock(id, name);
        modelBuilder.Add(buildingBlock, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, buildingBlock, buildingBlockAttribute, elements));
        if (hasModule)
        {
            modelBuilder.Add(module!, symbol);
            modelBuilder.Add(new DomainModule.ContainsBuildingBlock(module!, buildingBlock));
        }
        AddDescriptionTrait(symbol, buildingBlock, modelBuilder);
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

    protected abstract DomainBuildingBlock CreateBuildingBlock(string id, string name);

    protected virtual IEnumerable<Relation> GetRelations(ISymbol symbol, DomainBuildingBlock buildingBlock,
        AttributeData buildingBlockAttribute, ElementsProvider elements) => elements
        .For(symbol)
        .OfType<CodeStructure>()
        .Select(codeStructure => new DomainBuildingBlock.IsImplementedBy(buildingBlock, codeStructure));

    private static void AddDescriptionTrait(ISymbol symbol, DomainBuildingBlock buildingBlock,
        ModelBuilder modelBuilder)
    {
        var descriptionFile = GetDescriptionFile(symbol);
        if (descriptionFile == null)
            return;
        var descriptionTrait = new DomainBuildingBlockDescription(buildingBlock, descriptionFile);
        modelBuilder.Add(descriptionTrait);
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
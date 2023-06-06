using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.Ddd;

public abstract class DddBuildingBlockAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    protected abstract Type AttributeType { get; }
    
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        // TODO: Support for duplicated symbols (partial classes)
        if (!symbol.TryGetAttribute(AttributeType, out var buildingBlockAttribute))
            return;
        var name = buildingBlockAttribute.ConstructorArguments[0].Value?.ToString() ?? symbol.Name;
        var descriptionFile = GetDescriptionFile(symbol);
        var buildingBlock = CreateBuildingBlock(name, descriptionFile);
        modelBuilder.Add(buildingBlock, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, buildingBlock, elements));
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

    protected abstract DomainBuildingBlock CreateBuildingBlock(string name, FileInfo? descriptionFile);
    
    private static IEnumerable<Relation> GetRelations(ISymbol symbol, DomainBuildingBlock buildingBlock,
        ElementsProvider elements)
    {
        var module = elements.For(symbol.ContainingNamespace)
            .OfType<DomainModule>()
            .SingleOrDefault();
        if (module != null)
            yield return new DomainModule.ContainsBuildingBlock(module, buildingBlock);
    }

}
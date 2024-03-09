using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

// TODO: support for defining domain modules structure without namespaces
public class DomainModuleAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : FileAnalyzer, SymbolAnalyzer<INamespaceSymbol>
{
    public async Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder)
    {
        if (fileInfo.Directory is null)
            throw new InvalidOperationException();
        if (!fileInfo.Name.EndsWith("DomainModuleInfo.json"))
            return;
        await using var stream = fileInfo.Open(FileMode.Open);
        // TODO: JsonSerializerOptions configuration
        var moduleInfo = await JsonSerializer.DeserializeAsync<DomainModuleInfo>(stream,
            new JsonSerializerOptions());
        // TODO: warning logging
        if (moduleInfo is null)
            return;
        // TODO: Rethink adding same element from different analyzers (relations and symbols are no problem, but element's data are).
        var module = new DomainModule(HierarchyId.FromValue(moduleInfo.DomainModule.Dehumanize()));
        modelBuilder.Add(module, fileInfo.Directory);
        if (moduleInfo.DevelopmentOwner != null)
        {
            var team = new DevelopmentTeam(moduleInfo.DevelopmentOwner);
            modelBuilder.Add(team);
            modelBuilder.Add(new DevelopmentTeam.OwnsDomainModule(team, module));
        }
        if (moduleInfo.BusinessOwner != null)
        {
            var organizationalUnit = new BusinessOrganizationalUnit(moduleInfo.BusinessOwner);
            modelBuilder.Add(organizationalUnit);
            modelBuilder.Add(new BusinessOrganizationalUnit.OwnsDomainModule(organizationalUnit, module));
        }
    }

    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.IsExplicitlyIncludedInDomainModel())
            return;
        foreach (var namespaceSymbol in GetFullHierarchy(symbol))
        {
            if (!namespaceSymbol.IsExplicitlyIncludedInDomainModel())
            {
                Log.Warning("Namespace {namespace} implementing a part of domain modules hierarchy doesn't belong to domain model.",
                    namespaceSymbol.ToDisplayString());
                continue;
            }
            if (!modulesHierarchyResolver.TryFind(namespaceSymbol, out var hierarchyId))
                continue;
            var module = new DomainModule(hierarchyId.Value);
            modelBuilder.Add(module, namespaceSymbol);
            // TODO: relation to teams and business units defined at namespace level
            //  Requires parsing types within the namespace annotated with DevelopmentOwnerAttribute and ApplyOnNamespace == true.
            modelBuilder.Add(elements => GetRelationToParent(namespaceSymbol, module, elements));
            modelBuilder.Add(elements => GetRelationsToCodeStructures(namespaceSymbol, module, elements));
        }
    }

    private static IEnumerable<INamespaceSymbol> GetFullHierarchy(INamespaceSymbol symbol)
    {
        while (symbol is { IsGlobalNamespace: false })
        {
            yield return symbol;
            symbol = symbol.ContainingNamespace;
        }
    }

    private static IEnumerable<Relation> GetRelationToParent(ISymbol symbol, DomainModule module,
        ElementsProvider elements)
    {
        // TODO: warning logging if more than one
        var parentModule = elements
            .OfType<ElementInfo<DomainModule>>()
            // TODO: Checking up the hierarchy because some namespaces can be filtered out (e.g. layer name) 
            .Where(info => info.Symbols.Contains(symbol.ContainingNamespace))
            .Select(info => info.Element)
            .SingleOrDefault();
        if (module.Equals(parentModule))
            Log.Warning($"Domain module: {module} has relation: {nameof(DomainModule.ContainsDomainModule)} to itself");
        else if (parentModule != null)
            yield return new DomainModule.ContainsDomainModule(parentModule, module);
    }

    private static IEnumerable<Relation> GetRelationsToCodeStructures(ISymbol symbol, DomainModule module,
        ElementsProvider elements) => elements
        .For(symbol)
        .OfType<CodeStructure>()
        .Select(codeStructure => new DomainModule.IsImplementedBy(module, codeStructure));
}
using System.Text.Json;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.Domain;

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
        var hierarchyPath = HierarchyPath.FromValue(moduleInfo.DomainModule.Dehumanize());
        var module = new DomainModule(
            ElementId.Create<DomainModule>(hierarchyPath.Full),
            hierarchyPath);
        modelBuilder.Add(module, fileInfo.Directory);
        if (moduleInfo.DevelopmentOwner != null)
        {
            var team = new DevelopmentTeam(
                ElementId.Create<DevelopmentTeam>(moduleInfo.DevelopmentOwner.Dehumanize()),
                moduleInfo.DevelopmentOwner);
            modelBuilder.Add(team);
            modelBuilder.Add(new DevelopmentTeam.OwnsDomainModule(team, module));
        }
        if (moduleInfo.BusinessOwner != null)
        {
            var organizationalUnit = new BusinessOrganizationalUnit(
                ElementId.Create<BusinessOrganizationalUnit>(moduleInfo.BusinessOwner.Dehumanize()),
                moduleInfo.BusinessOwner);
            modelBuilder.Add(organizationalUnit);
            modelBuilder.Add(new BusinessOrganizationalUnit.OwnsDomainModule(organizationalUnit, module));
        }
    }

    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.IsExplicitlyIncludedInDomainModel())
            return;
        DomainModule? previousModule = null;
        while (symbol is { IsGlobalNamespace: false })
        {
            if (!symbol.IsExplicitlyIncludedInDomainModel())
            {
                Log.Warning("Namespace {namespace} implementing a part of domain modules hierarchy doesn't belong to domain model.",
                    symbol.ToDisplayString());
                symbol = symbol.ContainingNamespace;
                continue;
            }
            if (!modulesHierarchyResolver.TryFind(symbol, out var hierarchyPath))
            {
                symbol = symbol.ContainingNamespace;
                continue;
            }
            if (hierarchyPath.Equals(previousModule?.HierarchyPath))
            {
                symbol = symbol.ContainingNamespace;
                continue;
            }
            var module = new DomainModule(
                ElementId.Create<DomainModule>(hierarchyPath.Value.Full),
                hierarchyPath.Value);
            modelBuilder.Add(module, symbol);
            if (previousModule != null)
                modelBuilder.Add(new DomainModule.ContainsDomainModule(module, previousModule));
            // TODO: relation to teams and business units defined at namespace level
            //  Requires parsing types within the namespace annotated with DevelopmentOwnerAttribute and ApplyOnNamespace == true.
            var currentSymbol = symbol;
            modelBuilder.Add(elements => GetRelationsToCodeStructures(currentSymbol, module, elements));
            
            previousModule = module;
            symbol = symbol.ContainingNamespace;
        }
    }

    private static IEnumerable<Relation> GetRelationsToCodeStructures(ISymbol symbol, DomainModule module,
        ElementsProvider elements) => elements
        .For(symbol)
        .OfType<CodeStructure>()
        .Select(codeStructure => new DomainModule.IsImplementedBy(module, codeStructure));
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

// TODO: support for defining domain modules structure without namespaces
public class DomainModuleAnalyzer : FileAnalyzer, SymbolAnalyzer<INamespaceSymbol>
{
    private readonly DomainModuleFinder _domainModuleFinder;

    public DomainModuleAnalyzer(DomainModuleFinder domainModuleFinder) => _domainModuleFinder = domainModuleFinder;

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
        var module = new DomainModule(HierarchyId.FromValue(moduleInfo.DomainModule));
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
        if (!_domainModuleFinder.TryFind(symbol, out var module))
            return;
        modelBuilder.Add(module, symbol);
        // TODO: relation to teams and business units defined at namespace level
        //  Requires parsing types within the namespace annotated with DevelopmentOwnerAttribute and ApplyOnNamespace == true.
        modelBuilder.Add(elements => GetRelationToParent(symbol, module, elements));
    }

    private static IEnumerable<Relation> GetRelationToParent(ISymbol symbol, DomainModule module,
        ElementsProvider elements)
    {
        // TODO: warning logging if more than one
        var parentModule = elements
            .OfType<ElementInfo<DomainModule>>()
            .Where(info => info.Symbols.Contains(symbol.ContainingNamespace, SymbolEqualityComparer.Default))
            .Select(info => info.Element)
            .SingleOrDefault();
        if (parentModule != null)
            yield return new DomainModule.ContainsDomainModule(parentModule, module);
    }
}
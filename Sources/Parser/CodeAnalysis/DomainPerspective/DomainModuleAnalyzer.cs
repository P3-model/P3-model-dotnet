using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

public class DomainModuleAnalyzer : FileAnalyzer, SymbolAnalyzer<IAssemblySymbol>, SymbolAnalyzer<INamespaceSymbol>
{
    private readonly Func<INamespaceSymbol, bool> _isDomainModelNamespace;
    private readonly Func<INamespaceSymbol, string> _getModulesHierarchy;

    public DomainModuleAnalyzer(Func<INamespaceSymbol, bool> isDomainModelNamespace,
        Func<INamespaceSymbol, string> getModulesHierarchy)
    {
        _isDomainModelNamespace = isDomainModelNamespace;
        _getModulesHierarchy = getModulesHierarchy;
    }
    
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

    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(DomainModuleAttribute), out var attribute))
            return;
        var name = attribute.ConstructorArguments[0].Value?.ToString() ?? symbol.Name.Humanize(LetterCasing.Title);
        var module = new DomainModule(HierarchyId.FromValue(name));
        modelBuilder.Add(module, symbol);
    }

    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.IsGlobalNamespace || !_isDomainModelNamespace(symbol))
            return;
        var modulesHierarchy = _getModulesHierarchy(symbol);
        if (string.IsNullOrEmpty(modulesHierarchy))
            return;
        var module = new DomainModule(HierarchyId.FromValue(modulesHierarchy));
        modelBuilder.Add(module, symbol);
        modelBuilder.Add(elements => GetRelationToParent(symbol, module, elements));
    }

    private static IEnumerable<Relation> GetRelationToParent(ISymbol symbol, DomainModule module,
        ElementsProvider elements)
    {
        // TODO: warning logging if more than one
        var parentModule = elements
            .OfType<ElementInfo<DomainModule>>()
            .Where(info => info.Symbols.Contains(symbol.ContainingNamespace, SymbolEqualityComparer.Default) ||
                           info.Symbols.Contains(symbol.ContainingAssembly, SymbolEqualityComparer.Default) ||
                           symbol.SourcesAreInAny(info.Directories))
            .Select(info => info.Element)
            .SingleOrDefault();
        if (parentModule != null)
            yield return new DomainModule.ContainsDomainModule(parentModule, module);
    }
}
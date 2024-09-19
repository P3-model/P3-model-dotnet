using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Technology;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.Domain;

// TODO: support for defining domain modules structure without namespaces
public class NamespaceBasedDomainModuleAnalyzer(ImmutableArray<string> partsToSkip) :
    SymbolAnalyzer<INamespaceSymbol>,
    DomainModuleAnalyzer
{
    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder) => TryResolve(symbol, modelBuilder, out _);

    public bool TryResolve(ISymbol symbol, ModelBuilder modelBuilder,
        [NotNullWhen(true)] out DomainModule? domainModule)
    {
        if (TryGetNamespaceSymbol(symbol, out var namespaceSymbol))
            return TryResolve(namespaceSymbol, modelBuilder, out domainModule);
        domainModule = null;
        return false;
    }

    private bool TryResolve(INamespaceSymbol namespaceSymbol, ModelBuilder modelBuilder,
        [NotNullWhen(true)] out DomainModule? domainModule)
    {
        if (!namespaceSymbol.IsExplicitlyIncludedInDomainModel())
        {
            domainModule = null;
            return false;
        }
        if (!TryCreateModule(namespaceSymbol, modelBuilder, out domainModule))
            return false;
        CreateModulesHierarchy(namespaceSymbol.ContainingNamespace, domainModule, modelBuilder);
        return true;
    }

    private static bool TryGetNamespaceSymbol(ISymbol symbol, [NotNullWhen(true)] out INamespaceSymbol? namespaceSymbol)
    {
        namespaceSymbol = symbol switch
        {
            INamespaceSymbol ns => ns,
            INamedTypeSymbol ts => ts.ContainingNamespace,
            IMethodSymbol ms => ms.ContainingType.ContainingNamespace,
            _ => null
        };
        return namespaceSymbol != null;
    }

    private void CreateModulesHierarchy(INamespaceSymbol namespaceSymbol, DomainModule previousModule,
        ModelBuilder modelBuilder)
    {
        while (namespaceSymbol is { IsGlobalNamespace: false })
        {
            if (TryCreateModule(namespaceSymbol, previousModule, modelBuilder, out var currentModule))
                previousModule = currentModule;
            namespaceSymbol = namespaceSymbol.ContainingNamespace;
        }
    }

    private bool TryCreateModule(INamespaceSymbol namespaceSymbol, ModelBuilder modelBuilder,
        [NotNullWhen(true)] out DomainModule? domainModule) =>
        TryCreateModule(namespaceSymbol, null, modelBuilder, out domainModule);

    private bool TryCreateModule(INamespaceSymbol namespaceSymbol, DomainModule? previousModule,
        ModelBuilder modelBuilder, [NotNullWhen(true)] out DomainModule? createdModule)
    {
        if (previousModule != null && !namespaceSymbol.IsExplicitlyIncludedInDomainModel())
        {
            Log.Warning(
                "Namespace {namespace} implementing a part of domain modules hierarchy doesn't belong to domain model.",
                namespaceSymbol.ToDisplayString());
            createdModule = null;
            return false;
        }
        if (!TryFindHierarchyPath(namespaceSymbol, out var hierarchyPath))
        {
            createdModule = null;
            return false;
        }
        createdModule = new DomainModule(ElementId.Create<DomainModule>(hierarchyPath.Full), hierarchyPath);
        modelBuilder.Add(createdModule, namespaceSymbol);
        AddRelations(namespaceSymbol, createdModule, previousModule, modelBuilder);
        return true;
    }

    private bool TryFindHierarchyPath(INamespaceSymbol symbol, out HierarchyPath hierarchyPath)
    {
        var parts = new List<string>();
        while (!symbol.IsGlobalNamespace)
        {
            if (!IsSkippedNamespace(symbol))
                parts.Add(symbol.Name);
            symbol = symbol.ContainingNamespace;
        }
        if (parts.Count == 0)
        {
            hierarchyPath = default;
            return false;
        }
        parts.Reverse();
        hierarchyPath = HierarchyPath.FromParts(parts);
        return true;
    }

    private bool IsSkippedNamespace(INamespaceSymbol symbol) =>
        symbol.TryGetAttribute(typeof(SkipNamespaceInDomainModulesHierarchyAttribute), out _) ||
        partsToSkip.Any(partToSkip => symbol.Name.Equals(partToSkip, StringComparison.OrdinalIgnoreCase));

    private static void AddRelations(INamespaceSymbol namespaceSymbol,
        DomainModule currentModule,
        DomainModule? previousModule,
        ModelBuilder modelBuilder)
    {
        if (previousModule != null && !previousModule.Equals(currentModule))
            modelBuilder.Add(new DomainModule.ContainsDomainModule(currentModule, previousModule));
        modelBuilder.Add(elements => GetRelationsToCodeStructures(namespaceSymbol, currentModule, elements));
        // TODO: relation to teams and business units defined at namespace level
        //  Requires parsing types within the namespace annotated with DevelopmentOwnerAttribute and ApplyOnNamespace == true.
    }

    private static IEnumerable<Relation> GetRelationsToCodeStructures(ISymbol symbol, DomainModule module,
        ElementsProvider elements) => elements
        .For(symbol)
        .OfType<CodeStructure>()
        .Select(codeStructure => new DomainModule.IsImplementedBy(module, codeStructure));
}
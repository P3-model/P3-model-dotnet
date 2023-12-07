using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.People;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.People;

// TODO: support for defining business owners at assembly level
[UsedImplicitly]
public class BusinessOrganizationalUnitAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(BusinessOwnerAttribute), out var ownerAttribute))
            return;
        var applyOnNamespace = ownerAttribute.GetArgumentValue<bool>(
            nameof(BusinessOwnerAttribute.ApplyOnNamespace));
        if (!applyOnNamespace)
            return;
        var ns = symbol.ContainingNamespace;
        var name = ownerAttribute.GetConstructorArgumentValue<string>(nameof(BusinessOwnerAttribute.Name));
        var unit = new BusinessOrganizationalUnit(name);
        modelBuilder.Add(unit, ns);
        modelBuilder.Add(elements => GetRelations(ns, unit, elements));
    }

    private static IEnumerable<Relation> GetRelations(ISymbol symbol, BusinessOrganizationalUnit unit,
        ElementsProvider elements) =>
        elements.For(symbol)
            .OfType<DomainModule>()
            .Select(module => new BusinessOrganizationalUnit.OwnsDomainModule(unit, module));
}
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.People;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.People;

[UsedImplicitly]
public class DevelopmentTeamAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(DevelopmentOwnerAttribute), out var ownerAttribute))
            return;
        var applyOnNamespace = ownerAttribute.GetArgumentValue<bool>(
            nameof(DevelopmentOwnerAttribute.ApplyOnNamespace));
        if (!applyOnNamespace)
            return;
        var ns = symbol.ContainingNamespace;
        var name = ownerAttribute.GetConstructorArgumentValue<string>(nameof(DevelopmentOwnerAttribute.Name));
        var team = new DevelopmentTeam(name);
        modelBuilder.Add(team, ns);
        modelBuilder.Add(elements => GetRelations(ns, team, elements));
    }

    private static IEnumerable<Relation> GetRelations(ISymbol symbol, DevelopmentTeam team,
        ElementsProvider elements) =>
        elements.For(symbol)
            .OfType<DomainModule>()
            .Select(module => new DevelopmentTeam.OwnsDomainModule(team, module));
}
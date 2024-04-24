using System.Collections.Generic;
using System.Linq;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class TierAnalyzer : SymbolAnalyzer<IAssemblySymbol>
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(TierAttribute), out var tierAttribute))
            return;
        var name = tierAttribute.GetConstructorArgumentValue<string>(nameof(TierAttribute.Name));
        var tier = new Tier(
            ElementId.Create<Tier>(name.Dehumanize()),
            name.Humanize(LetterCasing.Title));
        modelBuilder.Add(tier, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, tier, elements));
    }

    private static IEnumerable<Relation> GetRelations(ISymbol symbol, Tier tier, ElementsProvider elements) =>
        elements
            .For(symbol)
            .OfType<DeployableUnit>()
            .Select(deployableUnit => new Tier.ContainsDeployableUnit(tier, deployableUnit));
}
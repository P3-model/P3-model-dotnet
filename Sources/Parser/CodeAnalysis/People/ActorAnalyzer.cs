using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.People;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.People;

[UsedImplicitly]
public class ActorAnalyzer : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    private static void Analyze(ISymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ActorAttribute), out var actorAttribute))
            return;
        var name = actorAttribute.GetConstructorArgumentValue<string>();
        var actor = new Actor(
            ElementId.Create<Actor>(name.Dehumanize()), 
            name.Humanize(LetterCasing.Title));
        modelBuilder.Add(actor, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, actor, elements));
    }
    
    private static IEnumerable<Relation> GetRelations(ISymbol symbol, Actor actor, ElementsProvider elements) => 
        elements.For(symbol)
            .OfType<UseCase>()
            .Select(useCase => new Actor.UsesUseCase(actor, useCase));
}
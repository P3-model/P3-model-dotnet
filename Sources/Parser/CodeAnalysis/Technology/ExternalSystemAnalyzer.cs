using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class ExternalSystemAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ExternalSystemAttribute), out var externalSystemAttribute))
            return;
        var name = externalSystemAttribute.GetConstructorArgumentValue<string?>() ?? symbol.GetFullName();
        var externalSystem = new ExternalSystem(
            ElementId.Create<ExternalSystem>(name.Dehumanize()),
            name.Humanize(LetterCasing.Title));
        modelBuilder.Add(externalSystem, symbol);
    }
}
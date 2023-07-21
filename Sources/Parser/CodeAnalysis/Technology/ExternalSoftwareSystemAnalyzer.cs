using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class ExternalSoftwareSystemAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ExternalSoftwareSystemAttribute), out var externalSystemAttribute))
            return;
        var name = externalSystemAttribute.GetConstructorArgumentValue<string?>() 
                   ?? symbol.Name.Humanize(LetterCasing.Title);
        var externalSystem = new ExternalSoftwareSystem(name);
        modelBuilder.Add(externalSystem, symbol);
    }
}
using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class ProcessAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ProcessAttribute), out var processAttribute))
            return;
        var name = processAttribute.GetConstructorArgumentValue<string>(nameof(ProcessAttribute.Name));
        var process = new Process(
            ElementId.Create<Process>(name.Dehumanize()), 
            name.Humanize(LetterCasing.Title));
        if (!processAttribute.TryGetNamedArgumentValue<bool>(nameof(ProcessAttribute.ApplyOnNamespace),
                out var applyOnNamespace))
            applyOnNamespace = false;
        modelBuilder.Add(process, applyOnNamespace ? symbol.ContainingNamespace : symbol);
    }
}
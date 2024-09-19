using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.CodeAnalysis.Domain;

public class DomainModuleAnalyzers(params DomainModuleAnalyzer[] analyzers) : DomainModuleAnalyzer
{
    private readonly IReadOnlyList<DomainModuleAnalyzer> _analyzers = analyzers;

    public bool TryResolve(ISymbol symbol, ModelBuilder modelBuilder, [NotNullWhen(true)] out DomainModule? domainModule)
    {
        foreach (var resolver in _analyzers)
        {
            if (resolver.TryResolve(symbol, modelBuilder, out domainModule))
                return true;
        }
        domainModule = default;
        return false;
    }
}
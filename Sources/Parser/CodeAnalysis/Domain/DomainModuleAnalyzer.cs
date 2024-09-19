using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.CodeAnalysis.Domain;

public interface DomainModuleAnalyzer
{
    bool TryResolve(ISymbol symbol, ModelBuilder modelBuilder, [NotNullWhen(true)] out DomainModule? domainModule);
}
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

public interface SymbolAnalyzer { }

public interface SymbolAnalyzer<in TSymbol> : SymbolAnalyzer
    where TSymbol : ISymbol
{
    void Analyze(TSymbol symbol, ModelBuilder modelBuilder);
}
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis;

// TODO: Analyze how to compare symbols.
// SymbolEqualityComparer.Default return false for same symbol from different compilations.
public class CompilationIndependentSymbolEqualityComparer :  IEqualityComparer<ISymbol?>
{
    public static readonly CompilationIndependentSymbolEqualityComparer Default = new();

    private CompilationIndependentSymbolEqualityComparer() { }

    public bool Equals(ISymbol? x, ISymbol? y) => x != null && 
                                                  y != null && 
                                                  x.ToDisplayString().Equals(y.ToDisplayString());

    public int GetHashCode(ISymbol obj) => obj.ToDisplayString().GetHashCode();
}
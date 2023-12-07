using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public interface DomainModuleFinder
{
    bool TryFind(ISymbol symbol, [NotNullWhen(true)] out DomainModule? module);
}
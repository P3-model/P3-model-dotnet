using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

public interface DomainModuleFinder
{
    bool TryFind(ISymbol symbol, [NotNullWhen(true)] out DomainModule? module);
}
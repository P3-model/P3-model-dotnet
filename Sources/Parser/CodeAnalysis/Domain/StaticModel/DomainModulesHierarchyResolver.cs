using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public interface DomainModulesHierarchyResolver
{
    bool TryFind(ISymbol symbol, [NotNullWhen(true)] out HierarchyPath? hierarchyPath);
}
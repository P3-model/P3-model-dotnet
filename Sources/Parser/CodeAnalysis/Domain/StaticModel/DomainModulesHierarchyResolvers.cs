using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public class DomainModulesHierarchyResolvers(params DomainModulesHierarchyResolver[] finders) : DomainModulesHierarchyResolver
{
    private readonly IReadOnlyList<DomainModulesHierarchyResolver> _finders = finders;

    public bool TryFind(ISymbol symbol, [NotNullWhen(true)] out HierarchyId? hierarchyId)
    {
        foreach (var finder in _finders)
        {
            if (finder.TryFind(symbol, out hierarchyId))
                return true;
        }
        hierarchyId = null;
        return false;
    }
}
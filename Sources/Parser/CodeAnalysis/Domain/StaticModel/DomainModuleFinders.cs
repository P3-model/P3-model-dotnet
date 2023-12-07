using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public class DomainModuleFinders : DomainModuleFinder
{
    private readonly IReadOnlyList<DomainModuleFinder> _finders;
    
    public DomainModuleFinders(params DomainModuleFinder[] finders) => _finders = finders;

    public bool TryFind(ISymbol symbol, [NotNullWhen(true)] out DomainModule? module)
    {
        foreach (var finder in _finders)
        {
            if (finder.TryFind(symbol, out module))
                return true;
        }
        module = null;
        return false;
    }
}
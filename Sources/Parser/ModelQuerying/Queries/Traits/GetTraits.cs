using System;
using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Traits;

public class GetTraits<TTrait> : TraitsQuery<TTrait>
    where TTrait : Trait
{
    private readonly Func<TTrait, bool>? _predicate;

    public GetTraits(Func<TTrait, bool>? predicate = null) => _predicate = predicate;

    public IReadOnlySet<TTrait> Execute(ModelGraph modelGraph) => _predicate is null
        ? modelGraph.TraitsOfType<TTrait>().ToHashSet()
        : modelGraph.TraitsOfType<TTrait>().Where(t => _predicate(t)).ToHashSet();
}
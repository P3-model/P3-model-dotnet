using System;
using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetElementsWithRelation<TSource, TRelation> : ElementsQuery<TSource>
    where TSource : Element
    where TRelation : RelationFrom<TSource>
{
    private readonly Func<TRelation, bool> _predicate;

    public GetElementsWithRelation(Func<TRelation, bool> predicate) => _predicate = predicate;

    public IReadOnlySet<TSource> ExecuteFor(ModelGraph modelGraph) => modelGraph
        .RelationsOfType<TRelation>()
        .Where(r => _predicate(r))
        .Select(r => r.Source)
        .ToHashSet();
}
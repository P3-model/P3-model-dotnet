using System;
using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetElementsBackRelatedToAny<TSource, TDestination, TRelation> : ElementsQuery<TSource>
    where TSource : class, Element
    where TDestination : class, Element, IEquatable<TDestination>
    where TRelation : Relation<TDestination, TSource>
{
    private readonly ElementsQuery<TDestination> _destinationQuery;
    private readonly Func<IEnumerable<TRelation>, IEnumerable<TRelation>>? _filter;

    public GetElementsBackRelatedToAny(ElementsQuery<TDestination> destinationQuery,
        Func<IEnumerable<TRelation>, IEnumerable<TRelation>>? filter)
    {
        _destinationQuery = destinationQuery;
        _filter = filter;
    }

    public IReadOnlySet<TSource> ExecuteFor(ModelGraph modelGraph) => _filter is null
        ? modelGraph
            .RelationsFrom<TDestination, TRelation>(_destinationQuery.ExecuteFor(modelGraph))
            .Select(r => r.Destination)
            .ToHashSet()
        : modelGraph
            .RelationsFrom<TDestination, TRelation>(_destinationQuery.ExecuteFor(modelGraph))
            .Apply(_filter)
            .Select(r => r.Destination)
            .ToHashSet();
}
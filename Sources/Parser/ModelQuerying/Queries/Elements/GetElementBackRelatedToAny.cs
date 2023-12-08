using System;
using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetElementBackRelatedToAny<TSource, TDestination, TRelation> : ElementQuery<TSource>
    where TSource : class, Element
    where TDestination : class, Element
    where TRelation : Relation<TDestination, TSource>
{
    private readonly ElementsQuery<TDestination> _destinationQuery;
    private readonly Func<IEnumerable<TRelation>, TRelation?> _filter;

    public GetElementBackRelatedToAny(ElementsQuery<TDestination> destinationQuery,
        Func<IEnumerable<TRelation>, TRelation?> filter)
    {
        _destinationQuery = destinationQuery;
        _filter = filter;
    }

    public TSource? ExecuteFor(ModelGraph modelGraph) => modelGraph
        .RelationsFrom<TDestination, TRelation>(_destinationQuery.ExecuteFor(modelGraph))
        .Apply(_filter)
        ?.Destination;
}
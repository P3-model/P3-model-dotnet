using System;
using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetElementRelatedToAny<TSource, TDestination, TRelation> : ElementQuery<TSource>
    where TSource : class, Element
    where TDestination : class, Element, IEquatable<TDestination>
    where TRelation : Relation<TSource, TDestination>
{
    private readonly ElementsQuery<TDestination> _destinationQuery;
    private readonly Func<IEnumerable<TRelation>, TRelation?> _filter;

    public GetElementRelatedToAny(ElementsQuery<TDestination> destinationQuery,
        Func<IEnumerable<TRelation>, TRelation?> filter)
    {
        _destinationQuery = destinationQuery;
        _filter = filter;
    }

    public TSource? ExecuteFor(ModelGraph modelGraph) => modelGraph
        .RelationsTo<TDestination, TRelation>(_destinationQuery.ExecuteFor(modelGraph))
        .Apply(_filter)
        ?.Source;
}
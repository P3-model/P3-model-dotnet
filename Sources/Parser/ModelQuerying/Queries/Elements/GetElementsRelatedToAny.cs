using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Marker argument for ModelGraph methods")]
public class GetElementsRelatedToAny<TSource, TDestination, TRelation> : ElementsQuery<TSource>
    where TSource : class, Element
    where TDestination : class, Element
    where TRelation : Relation<TSource, TDestination>
{
    private readonly ElementsQuery<TDestination> _destinationQuery;
    private readonly Func<IEnumerable<TRelation>, IEnumerable<TRelation>>? _filter;

    public GetElementsRelatedToAny(ElementsQuery<TDestination> destinationQuery,
        Func<IEnumerable<TRelation>, IEnumerable<TRelation>>? filter)
    {
        _destinationQuery = destinationQuery;
        _filter = filter;
    }

    public IReadOnlySet<TSource> ExecuteFor(ModelGraph modelGraph) => _filter is null
        ? modelGraph
            .RelationsTo<TDestination, TRelation>(_destinationQuery.ExecuteFor(modelGraph))
            .Select(r => r.Source)
            .ToHashSet()
        : modelGraph
            .RelationsTo<TDestination, TRelation>(_destinationQuery.ExecuteFor(modelGraph))
            .Apply(_filter)
            .Select(r => r.Source)
            .ToHashSet();
}
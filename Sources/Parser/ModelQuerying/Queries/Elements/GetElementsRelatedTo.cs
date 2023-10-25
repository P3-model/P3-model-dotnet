using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Marker argument for ModelGraph methods")]
public class GetElementsRelatedTo<TSource, TDestination, TRelation> : ElementsQuery<TSource>
    where TSource : class, Element
    where TDestination : class, Element, IEquatable<TDestination>
    where TRelation : Relation<TSource, TDestination>
{
    private readonly TDestination _destination;

    public GetElementsRelatedTo(TDestination destination) => _destination = destination;

    public IReadOnlySet<TSource> ExecuteFor(ModelGraph modelGraph) => modelGraph
        .RelationsTo<TDestination, TRelation>(_destination)
        .Select(r => r.Source)
        .ToHashSet();
}
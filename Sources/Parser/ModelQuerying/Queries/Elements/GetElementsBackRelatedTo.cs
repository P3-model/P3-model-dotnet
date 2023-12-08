using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetElementsBackRelatedTo<TSource, TDestination, TRelation> : ElementsQuery<TSource>
    where TSource : class, Element
    where TDestination : class, Element
    where TRelation : Relation<TDestination, TSource>
{
    private readonly TDestination _destination;

    public GetElementsBackRelatedTo(TDestination destination) => _destination = destination;

    public IReadOnlySet<TSource> ExecuteFor(ModelGraph modelGraph) => modelGraph
        .RelationsFrom<TDestination, TRelation>(_destination)
        .Select(r => r.Destination)
        .ToHashSet();
}
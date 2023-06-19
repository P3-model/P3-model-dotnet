using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying;

public class ModelGraph
{
    private readonly Model _model;
    
    public ModelCache Cache { get; }

    private ModelGraph(Model model)
    {
        _model = model;
        Cache = new ModelCache(model);
    }

    public static ModelGraph From(Model model) => new(model);

    [PublicAPI]
    public TElement Execute<TElement>(GetElementOfType<TElement> query)
        where TElement : Element =>
        _model.Elements
            .OfType<TElement>()
            .Single(e => query.Predicate?.Invoke(e) ?? true);
    
    [PublicAPI]
    public IReadOnlySet<TElement> Execute<TElement>(GetElementsOfType<TElement> query)
        where TElement : Element =>
        _model.Elements
            .OfType<TElement>()
            .Where(e => query.Predicate?.Invoke(e) ?? true)
            .ToHashSet();
    
    [PublicAPI]
    public IReadOnlySet<TSource> Execute<TSource, TDestination, TRelation>(
        GetElementsRelatedTo<TSource, TDestination, TRelation> query)
        where TSource : Element
        where TDestination : Element, IEquatable<TDestination>
        where TRelation : Relation<TSource, TDestination> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => r.Destination.Equals(query.Destination))
            .Select(r => r.Source)
            .ToHashSet();

    [PublicAPI]
    public IReadOnlySet<TSource> Execute<TSource, TDestination, TRelation>(
        GetElementsBackRelatedTo<TSource, TDestination, TRelation> query)
        where TSource : Element
        where TDestination : Element, IEquatable<TDestination>
        where TRelation : Relation<TDestination, TSource> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => r.Source.Equals(query.Destination))
            .Select(r => r.Destination)
            .ToHashSet();

    [PublicAPI]
    public IReadOnlySet<TSource> Execute<TSource, TDestination, TRelation>(
        GetElementsRelatedToAny<TSource, TDestination, TRelation> query)
        where TSource : Element
        where TDestination : Element, IEquatable<TDestination>
        where TRelation : Relation<TSource, TDestination> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => query.Destinations.Contains(r.Destination))
            .Select(r => r.Source)
            .ToHashSet();

    [PublicAPI]
    public IReadOnlySet<TSource> Execute<TSource, TDestination, TRelation>(
        GetElementsBackRelatedToAny<TSource, TDestination, TRelation> query)
        where TSource : Element
        where TDestination : Element, IEquatable<TDestination>
        where TRelation : Relation<TDestination, TSource> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => query.Destinations.Contains(r.Source))
            .Select(r => r.Destination)
            .ToHashSet();

    [PublicAPI]
    public IReadOnlySet<TSource> Execute<TSource, TRelation>(
        GetElementsWithRelation<TSource, TRelation> query)
        where TSource : Element
        where TRelation : RelationFrom<TSource> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => query.Predicate(r))
            .Select(r => r.Source)
            .ToHashSet();
    
    [PublicAPI]
    public IReadOnlySet<TRelation> Execute<TRelation>(GetRelations<TRelation> query)
        where TRelation : Relation =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r =>  query.Predicate?.Invoke(r) ?? true)
            .ToHashSet();
    
    [PublicAPI]
    public IReadOnlySet<TTrait> Execute<TTrait>(GetTraits<TTrait> query)
        where TTrait : Trait =>
        _model.Traits
            .OfType<TTrait>()
            .Where(r =>  query.Predicate?.Invoke(r) ?? true)
            .ToHashSet();
}
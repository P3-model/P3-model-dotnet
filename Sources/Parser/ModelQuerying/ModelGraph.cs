using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelQuerying.Queries.Elements;
using P3Model.Parser.ModelQuerying.Queries.Hierarchies;
using P3Model.Parser.ModelQuerying.Queries.Relations;
using P3Model.Parser.ModelQuerying.Queries.Traits;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying;

public class ModelGraph
{
    private readonly Model _model;

    private ModelGraph(Model model) => _model = model;

    public static ModelGraph From(Model model) => new(model);

    public IEnumerable<TElement> ElementsOfType<TElement>() where TElement : Element =>
        _model.Elements.OfType<TElement>();

    public IEnumerable<TRelation> RelationsOfType<TRelation>()
        where TRelation : Relation =>
        _model.Relations.OfType<TRelation>();

    public Hierarchy<TElement> HierarchyFor<TElement, TRelation>()
        where TElement : HierarchyElement
        where TRelation : HierarchyRelation<TElement> => Hierarchy<TElement>.Create(RelationsOfType<TRelation>());

    public IEnumerable<TRelation> RelationsFrom<TElement, TRelation>(TElement element)
        where TElement : Element, IEquatable<TElement>
        where TRelation : RelationFrom<TElement> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => r.Source.Equals(element));
    
    public IEnumerable<TRelation> RelationsFrom<TElement, TRelation>(IReadOnlySet<TElement> elements)
        where TElement : Element, IEquatable<TElement>
        where TRelation : RelationFrom<TElement> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => elements.Contains(r.Source));

    public IEnumerable<TRelation> RelationsTo<TElement, TRelation>(TElement element)
        where TElement : Element, IEquatable<TElement>
        where TRelation : RelationTo<TElement> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => r.Destination.Equals(element));
    
    public IEnumerable<TRelation> RelationsTo<TElement, TRelation>(IReadOnlySet<TElement> elements)
        where TElement : Element, IEquatable<TElement>
        where TRelation : RelationTo<TElement> =>
        _model.Relations
            .OfType<TRelation>()
            .Where(r => elements.Contains(r.Destination));
    
    public IEnumerable<TTrait> TraitsOfType<TTrait>()
        where TTrait : Trait =>
        _model.Traits.OfType<TTrait>();

    [PublicAPI]
    public TElement? Execute<TElement>(Func<QueryBuilder, ElementQuery<TElement>> configure)
        where TElement : Element
    {
        var queryBuilder = new QueryBuilder();
        var query = configure(queryBuilder);
        return query.ExecuteFor(this);
    }
    
    [PublicAPI]
    public IReadOnlySet<TElement> Execute<TElement>(Func<QueryBuilder, ElementsQuery<TElement>> configure)
        where TElement : Element
    {
        var queryBuilder = new QueryBuilder();
        var query = configure(queryBuilder);
        return query.ExecuteFor(this);
    }
    
    [PublicAPI]
    public Hierarchy<TElement> Execute<TElement>(Func<QueryBuilder, HierarchyQuery<TElement>> configure)
        where TElement : HierarchyElement
    {
        var queryBuilder = new QueryBuilder();
        var query = configure(queryBuilder);
        return query.Execute(this);
    }
    
    [PublicAPI]
    public IReadOnlySet<TRelation> Execute<TRelation>(Func<QueryBuilder, RelationsQuery<TRelation>> configure)
        where TRelation : Relation
    {
        var queryBuilder = new QueryBuilder();
        var query = configure(queryBuilder);
        return query.Execute(this);
    }
    
    [PublicAPI]
    public IReadOnlySet<TTrait> Execute<TTrait>(Func<QueryBuilder, TraitsQuery<TTrait>> configure)
        where TTrait : Trait
    {
        var queryBuilder = new QueryBuilder();
        var query = configure(queryBuilder);
        return query.Execute(this);
    }

    // [PublicAPI]
    // public Hierarchy<TElement> Execute<TElement, TRelation>(
    //     GetHierarchy<TElement, TRelation> query)
    //     where TElement : HierarchyElement
    //     where TRelation : HierarchyRelation<TElement> =>
    //     Hierarchy<TElement>.Create(_model.Relations.OfType<TRelation>());
    //
    // [PublicAPI]
    // public IReadOnlySet<TSource> Execute<TSource, TDestination, THierarchyRelation, TRelation>(
    //     GetElementsRelatedToDescendants<TSource, TDestination, THierarchyRelation, TRelation> query)
    //     where TSource : Element
    //     where TDestination : HierarchyElement, IEquatable<TDestination>
    //     where THierarchyRelation : HierarchyRelation<TDestination>
    //     where TRelation : Relation<TSource, TDestination>
    // {
    //     var hierarchy = Hierarchy<TDestination>.Create(_model.Relations.OfType<THierarchyRelation>());
    //     var descendants = hierarchy.GetDescendantsFor(query.Destination);
    //     return _model.Relations.OfType<TRelation>()
    //         .Where(r => descendants.Contains(r.Destination) ||
    //                     (query.IncludeSelf && r.Destination.Equals(query.Destination)))
    //         .Select(r => r.Source)
    //         .ToHashSet();
    // }
    //
    // [PublicAPI]
    // public IReadOnlySet<TSource> Execute<TSource, TDestination, THierarchyRelation, TRelation>(
    //     GetElementsBackRelatedToDescendants<TSource, TDestination, THierarchyRelation, TRelation> query)
    //     where TSource : Element
    //     where TDestination : HierarchyElement, IEquatable<TDestination>
    //     where THierarchyRelation : HierarchyRelation<TDestination>
    //     where TRelation : Relation<TDestination, TSource>
    // {
    //     var hierarchy = Hierarchy<TDestination>.Create(_model.Relations.OfType<THierarchyRelation>());
    //     var descendants = hierarchy.GetDescendantsFor(query.Destination);
    //     return _model.Relations.OfType<TRelation>()
    //         .Where(r => descendants.Contains(r.Source) ||
    //                     (query.IncludeSelf && r.Source.Equals(query.Destination)))
    //         .Select(r => r.Destination)
    //         .ToHashSet();
    // }
}
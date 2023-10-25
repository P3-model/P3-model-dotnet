using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying.Queries.Elements;
using P3Model.Parser.ModelQuerying.Queries.Hierarchies;
using P3Model.Parser.ModelQuerying.Queries.Relations;
using P3Model.Parser.ModelQuerying.Queries.Traits;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;

[SuppressMessage("Performance", "CA1822", Justification = "Needed for builder pattern")]
public class QueryBuilder
{
    [PublicAPI]
    public GetElement<TElement> SingleElement<TElement>(Func<TElement, bool>? predicate = null)
        where TElement : Element => new(predicate);

    [PublicAPI]
    public GetElements<TElement> AllElements<TElement>(Func<TElement, bool>? predicate = null)
        where TElement : Element => new(predicate);

    [PublicAPI]
    public SelectRelatedElements<TElement> Elements<TElement>()
        where TElement : class, Element => new();

    [PublicAPI]
    public GetHierarchy<TElement, TRelation> Hierarchy<TElement, TRelation>()
        where TElement : class, HierarchyElement
        where TRelation : HierarchyRelation<TElement> => new();
    
    [PublicAPI]
    public GetAncestors<TElement, TRelation> Ancestors<TElement, TRelation>(TElement element)
        where TElement : class, HierarchyElement
        where TRelation : HierarchyRelation<TElement> => new(element, false);
    
    [PublicAPI]
    public GetAncestors<TElement, TRelation> AncestorsAndSelf<TElement, TRelation>(TElement element)
        where TElement : class, HierarchyElement
        where TRelation : HierarchyRelation<TElement> => new(element, true);
    
    [PublicAPI]
    public GetDescendants<TElement, TRelation> Descendants<TElement, TRelation>(TElement element)
        where TElement : class, HierarchyElement
        where TRelation : HierarchyRelation<TElement> => new(element, false);
    
    [PublicAPI]
    public GetDescendants<TElement, TRelation> DescendantsAndSelf<TElement, TRelation>(TElement element)
        where TElement : class, HierarchyElement
        where TRelation : HierarchyRelation<TElement> => new(element, true);

    [PublicAPI]
    public GetRelations<TRelation> Relations<TRelation>(Func<TRelation, bool>? predicate = null)
        where TRelation : Relation => new(predicate);

    [PublicAPI]
    public GetTraits<TTrait> Traits<TTrait>(Func<TTrait, bool>? predicate = null)
        where TTrait : Trait => new(predicate);

    public readonly struct SelectRelatedElements<TElement>
        where TElement : class, Element
    {
        [PublicAPI]
        public SelectRelation<TElement, TDestination> RelatedTo<TDestination>(TDestination destination)
            where TDestination : class, Element, IEquatable<TDestination> => new(destination);

        [PublicAPI]
        public SelectElementRelationToAny<TElement, TDestination> RelatedToAny<TDestination>(
            IReadOnlySet<TDestination> destinations)
            where TDestination : class, Element, IEquatable<TDestination> => 
            new(new GetStaticElements<TDestination>(destinations));
        
        [PublicAPI]
        public SelectElementRelationToAny<TElement, TDestination> RelatedToAny<TDestination>(
            Func<QueryBuilder, ElementsQuery<TDestination>> configure)
            where TDestination : class, Element, IEquatable<TDestination>
        {
            var subQueryBuilder = new QueryBuilder();
            var subQuery = configure(subQueryBuilder);
            return new SelectElementRelationToAny<TElement, TDestination>(subQuery);
        }

        [PublicAPI]
        public GetElementsWithRelation<TElement, TRelation> WithRelation<TRelation>(
            Func<TRelation, bool> predicate)
            where TRelation : RelationFrom<TElement> => new(predicate);
    }

    public readonly struct SelectRelation<TSource, TDestination> where TSource : class, Element
        where TDestination : class, Element, IEquatable<TDestination>
    {
        private readonly TDestination _destination;

        public SelectRelation(TDestination destination) => _destination = destination;

        [PublicAPI]
        public GetElementsRelatedTo<TSource, TDestination, TRelation> ByRelation<TRelation>()
            where TRelation : Relation<TSource, TDestination> => new(_destination);

        [PublicAPI]
        public GetElementsBackRelatedTo<TSource, TDestination, TRelation> ByReverseRelation<TRelation>()
            where TRelation : Relation<TDestination, TSource> => new(_destination);
    }

    public readonly struct SelectElementRelationToAny<TSource, TDestination>
        where TSource : class, Element
        where TDestination : class, Element, IEquatable<TDestination>
    {
        private readonly ElementsQuery<TDestination> _destinationQuery;

        public SelectElementRelationToAny(ElementsQuery<TDestination> destinationQuery) => 
            _destinationQuery = destinationQuery;

        [PublicAPI]
        public GetElementsRelatedToAny<TSource, TDestination, TRelation> ByRelation<TRelation>(
            Func<IEnumerable<TRelation>, IEnumerable<TRelation>>? filter = null)
            where TRelation : Relation<TSource, TDestination> => new(_destinationQuery, filter);
        
        [PublicAPI]
        public GetElementRelatedToAny<TSource, TDestination, TRelation> ByRelation<TRelation>(
            Func<IEnumerable<TRelation>, TRelation?> filter)
            where TRelation : Relation<TSource, TDestination> => new(_destinationQuery, filter);

        [PublicAPI]
        public GetElementsBackRelatedToAny<TSource, TDestination, TRelation> ByReverseRelation<TRelation>(
            Func<IEnumerable<TRelation>, IEnumerable<TRelation>>? filter = null)
            where TRelation : Relation<TDestination, TSource> => new(_destinationQuery, filter);
        
        [PublicAPI]
        public GetElementBackRelatedToAny<TSource, TDestination, TRelation> ByReverseRelation<TRelation>(
            Func<IEnumerable<TRelation>, TRelation?> filter)
            where TRelation : Relation<TDestination, TSource> => new(_destinationQuery, filter);
    }
}
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;

public readonly record struct GetTraits<TTrait>(Func<TTrait, bool>? Predicate = null);

public class Query
{
    [PublicAPI]
    public static SelectElements<TSource> Elements<TSource>() 
        where TSource : Element => new();

    [PublicAPI]
    public static GetRelations<TRelation> Relations<TRelation>(Func<TRelation, bool>? predicate = null) 
        where TRelation : Relation => new(predicate);
    
    [PublicAPI]
    public static GetTraits<TTrait> Traits<TTrait>(Func<TTrait, bool>? predicate = null) 
        where TTrait : Trait => new(predicate);

    public readonly record struct SelectElements<TSource>
        where TSource : Element
    {
        [PublicAPI]
        public GetElementOfType<TSource> Single(Func<TSource, bool>? predicate = null) => new(predicate);
        
        public GetElementsOfType<TSource> All(Func<TSource, bool>? predicate = null) => new(predicate);

        [PublicAPI]
        public SelectElementRelation<TSource, TDestination> RelatedTo<TDestination>(TDestination destination)
            where TDestination : Element => new(destination);

        [PublicAPI]
        public SelectElementRelationToAny<TSource, TDestination> RelatedTo<TDestination>(
            IReadOnlySet<TDestination> destinations)
            where TDestination : Element => new(destinations);

        [PublicAPI]
        public GetElementsWithRelation<TSource, TRelation> WithRelation<TRelation>(
            Func<TRelation, bool> predicate)
            where TRelation : RelationFrom<TSource> => new(predicate);
    }

    public readonly record struct SelectElementRelation<TSource, TDestination>(TDestination Destination)
        where TSource : Element
        where TDestination : Element
    {
        [PublicAPI]
        public GetElementsRelatedTo<TSource, TDestination, TRelation> ByRelation<TRelation>()
            where TRelation : Relation<TSource, TDestination> => new(Destination);

        [PublicAPI]
        public GetElementsBackRelatedTo<TSource, TDestination, TRelation> ByReverseRelation<TRelation>()
            where TRelation : Relation<TDestination, TSource> => new(Destination);
    }

    public readonly record struct SelectElementRelationToAny<TSource, TDestination>(
        IReadOnlySet<TDestination> Destinations)
        where TSource : Element
        where TDestination : Element
    {
        [PublicAPI]
        public GetElementsRelatedToAny<TSource, TDestination, TRelation> ByRelation<TRelation>()
            where TRelation : Relation<TSource, TDestination> => new(Destinations);

        [PublicAPI]
        public GetElementsBackRelatedToAny<TSource, TDestination, TRelation> ByReverseRelation<TRelation>()
            where TRelation : Relation<TDestination, TSource> => new(Destinations);
    }
}
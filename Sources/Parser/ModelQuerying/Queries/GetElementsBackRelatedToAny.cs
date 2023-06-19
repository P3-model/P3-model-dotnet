using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;


[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Marker argument for ModelGraph methods")]
public readonly record struct GetElementsBackRelatedToAny<TSource, TDestination, TRelation>(
    IReadOnlySet<TDestination> Destinations)
    where TSource : Element
    where TDestination : Element
    where TRelation : Relation<TDestination, TSource>;
using System.Diagnostics.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;

[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Marker argument for ModelGraph methods")]
public readonly record struct GetElementsRelatedTo<TSource, TDestination, TRelation>(TDestination Destination)
    where TSource : Element
    where TDestination : Element
    where TRelation : Relation<TSource, TDestination>;
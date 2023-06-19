using System;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;

public readonly record struct GetElementsWithRelation<TSource, TRelation>(
    Func<TRelation, bool> Predicate)
    where TSource : Element
    where TRelation : RelationFrom<TSource>;
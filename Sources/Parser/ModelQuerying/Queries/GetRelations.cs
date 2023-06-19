using System;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;

public readonly record struct GetRelations<TRelation>(Func<TRelation, bool>? Predicate)
    where TRelation : Relation;
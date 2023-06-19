using System;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries;

public readonly record struct GetElementOfType<TSource>(Func<TSource, bool>? Predicate)
    where TSource : Element;
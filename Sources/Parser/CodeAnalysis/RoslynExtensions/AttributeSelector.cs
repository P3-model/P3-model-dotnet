using System;
using JetBrains.Annotations;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

public readonly record struct AttributeSelector(Type Type, bool IncludeBaseTypes)
{
    [PublicAPI]
    public static AttributeSelector Exact(Type type) => new(type, false);
    [PublicAPI]
    public static AttributeSelector WithBaseTypes(Type type) => new(type, true);
    
    public static implicit operator AttributeSelector(Type type) => Exact(type);
}
using System;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

[Flags]
public enum GetAttributeOptions
{
    Default = 0,
    IncludeAttributeBaseTypes = 1
}
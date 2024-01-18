using System;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

[Flags]
public enum GetAttributeOptions
{
    Direct = 0,
    FromBaseClasses = 1,
    FromAllInterfaces = 2,
    IncludeAttributeBaseTypes = 4,
}
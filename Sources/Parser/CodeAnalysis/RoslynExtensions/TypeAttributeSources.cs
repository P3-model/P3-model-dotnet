using System;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

[Flags]
public enum TypeAttributeSources
{
    Self = 1,
    BaseClasses = 2,
    AllInterfaces = 4
}
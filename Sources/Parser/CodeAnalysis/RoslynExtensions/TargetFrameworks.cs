using System;

namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

[Flags]
public enum TargetFrameworks
{
    Net60 = 1,
    Net70 = 2,
    Net80 = 4,
    All = Net60 | Net70 | Net80
}
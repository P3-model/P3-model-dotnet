using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum FlowchartDirection
{
    TB,
    BT,
    RL,
    LR
}
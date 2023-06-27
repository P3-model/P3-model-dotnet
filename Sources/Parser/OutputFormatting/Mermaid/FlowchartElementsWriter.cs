using System;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[PublicAPI]
public interface FlowchartElementsWriter
{
    string WriteRectangle(string name, Style style = Style.Default);
    string WriteStadiumShape(string name, Style style = Style.Default);
    string WriteHexagon(string name, Style style = Style.Default);
    string WriteCylinder(string name, Style style = Style.Default);
    string WriteCircle(string name, Style style = Style.Default);
    string WriteRhombus(string name, Style style = Style.Default);
    void WriteOpenLink(string sourceId, string destinationId, LineStyle lineStyle);

    void WriteOpenLink(string sourceId, string destinationId, string? text = null,
        LineStyle lineStyle = LineStyle.Normal);

    void WriteArrow(string sourceId, string destinationId, LineStyle lineStyle);
    void WriteArrow(string sourceId, string destinationId, string? text = null, LineStyle lineStyle = LineStyle.Normal);
    void WriteBackwardArrow(string sourceId, string destinationId, LineStyle lineStyle);

    void WriteBackwardArrow(string sourceId, string destinationId, string? text,
        LineStyle lineStyle = LineStyle.Normal);

    void WriteInvisibleLink(string sourceId, string destinationId);
    string WriteSubgraph(string title, Action<FlowchartElementsWriter> writeElements);
    string WriteSubgraph(string title, FlowchartDirection direction, Action<FlowchartElementsWriter> writeElements);
}
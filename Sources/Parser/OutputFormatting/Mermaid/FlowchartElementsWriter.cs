using System;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[PublicAPI]
public interface FlowchartElementsWriter
{
    int WriteRectangle(string name, Style style = Style.Default);
    int WriteStadiumShape(string name, Style style = Style.Default);
    public int WriteCylinder(string name, Style style = Style.Default);
    public int WriteCircle(string name, Style style = Style.Default);
    public int WriteRhombus(string name, Style style = Style.Default);
    
    public void WriteOpenLink(int sourceId, int destinationId, LineStyle lineStyle);

    public void WriteOpenLink(int sourceId, int destinationId, string? text = null,
        LineStyle lineStyle = LineStyle.Normal);

    public void WriteArrow(int sourceId, int destinationId, LineStyle lineStyle);

    public void WriteArrow(int sourceId, int destinationId, string? text = null,
        LineStyle lineStyle = LineStyle.Normal);

    void WriteSubgraph(string title, Action<FlowchartElementsWriter> writeElements);
}
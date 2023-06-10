using System;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[PublicAPI]
public interface FlowchartElementsWriter
{
    int WriteRectangle(string name);
    int WriteStadiumShape(string name);
    public int WriteCylinder(string name);
    public int WriteCircle(string name);
    public int WriteRhombus(string name);
    public void WriteOpenLink(int sourceId, int destinationId, LineStyle lineStyle = LineStyle.Normal);
    public void WriteArrow(int sourceId, int destinationId, LineStyle lineStyle = LineStyle.Normal);
    void WriteSubgraph(string title, Action<FlowchartElementsWriter> writeElements);
}
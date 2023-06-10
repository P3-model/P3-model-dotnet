using System;
using System.IO;

namespace P3Model.Parser.OutputFormatting.Mermaid;

internal class FlowchartWriter : FlowchartElementsWriter
{
    private readonly StreamWriter _writer;
    private int _shapeId;
    private int _indent;

    public FlowchartWriter(StreamWriter writer) => _writer = writer;

    public void WriteDiagram(Action<FlowchartElementsWriter> addElements)
    {
        WriteLineIndented("```mermaid");
        _indent = 1;
        WriteLineIndented("flowchart");
        _indent = 2;
        addElements(this);
        _indent = 0;
        WriteLineIndented("```");
    }

    public int WriteRectangle(string name) => WriteShape($"({name})");
    public int WriteStadiumShape(string name) => WriteShape($"([{name}])");

    public int WriteCylinder(string name) => WriteShape($"[({name})]");

    public int WriteCircle(string name) => WriteShape($"(({name}))");

    public int WriteRhombus(string name) => WriteShape($"{{{name}}}");

    private int WriteShape(string value)
    {
        var id = _shapeId++;
        WriteLineIndented($"{id}{value}");
        return id;
    }

    public void WriteOpenLink(int sourceId, int destinationId, LineStyle lineStyle = LineStyle.Normal) => 
        WriteLink(sourceId, destinationId, GetOpenLinkSymbol(lineStyle));

    private static string GetOpenLinkSymbol(LineStyle lineStyle) => lineStyle switch
    {
        LineStyle.Normal => "---",
        LineStyle.Dotted => "-.-",
        LineStyle.Thick => "===",
        _ => throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null)
    };
    
    public void WriteArrow(int sourceId, int destinationId, LineStyle lineStyle = LineStyle.Normal) => 
        WriteLink(sourceId, destinationId, GetArrowSymbol(lineStyle));

    private static string GetArrowSymbol(LineStyle lineStyle) => lineStyle switch
    {
        LineStyle.Normal => "-->",
        LineStyle.Dotted => "-.->",
        LineStyle.Thick => "==>",
        _ => throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null)
    };
    
    private void WriteLink(int sourceId, int destinationId, string linkSymbol) =>
        WriteLineIndented($"{sourceId}{linkSymbol}{destinationId}");

    public void WriteSubgraph(string title, Action<FlowchartElementsWriter> writeElements)
    {
        WriteLineIndented($"subgraph {title}");
        _indent++;
        writeElements(this);
        _indent--;
        WriteLineIndented("end");
    }

    private void WriteLineIndented(string value)
    {
        _writer.Write(new string(' ', _indent * 2));
        _writer.WriteLine(value);
    }
}
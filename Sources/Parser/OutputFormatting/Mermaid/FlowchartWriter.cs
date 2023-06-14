using System;
using System.Collections.Generic;
using System.IO;

namespace P3Model.Parser.OutputFormatting.Mermaid;

internal class FlowchartWriter : FlowchartElementsWriter
{
    private static readonly Dictionary<Style, string> StyleDefinitions = new()
    {
        { Style.DomainPerspective, "stroke:#009900" },
        { Style.TechnologyPerspective, "stroke:#1F41EB" },
        { Style.PeoplePerspective, "stroke:#FFF014" }
    };

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
        WriteStyleDefinitions();
        _indent = 0;
        WriteLineIndented("```");
    }

    private void WriteStyleDefinitions()
    {
        foreach (var (name, def) in StyleDefinitions)
            WriteLineIndented($"classDef {name} {def}");
    }

    public int WriteRectangle(string name, Style style = Style.Default) =>
        WriteShape($"({name})", style);

    public int WriteStadiumShape(string name, Style style = Style.Default) =>
        WriteShape($"([{name}])", style);

    public int WriteCylinder(string name, Style style = Style.Default) =>
        WriteShape($"[({name})]", style);

    public int WriteCircle(string name, Style style = Style.Default) =>
        WriteShape($"(({name}))", style);

    public int WriteRhombus(string name, Style style = Style.Default) =>
        WriteShape($"{{{name}}}", style);

    private int WriteShape(string value, Style style)
    {
        var id = _shapeId++;
        WriteLineIndented($"{id}{value}");
        if (style != Style.Default)
            WriteLineIndented($"class {id} {style}");
        return id;
    }

    public void WriteOpenLink(int sourceId, int destinationId, LineStyle lineStyle) =>
        WriteOpenLink(sourceId, destinationId, null, lineStyle);

    public void WriteOpenLink(int sourceId, int destinationId, string? text = null,
        LineStyle lineStyle = LineStyle.Normal) =>
        WriteLink(sourceId, destinationId, GetOpenLinkSymbol(lineStyle), text);

    private static string GetOpenLinkSymbol(LineStyle lineStyle) => lineStyle switch
    {
        LineStyle.Normal => "---",
        LineStyle.Dotted => "-.-",
        LineStyle.Thick => "===",
        _ => throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null)
    };

    public void WriteArrow(int sourceId, int destinationId, LineStyle lineStyle) =>
        WriteArrow(sourceId, destinationId, null, lineStyle);

    public void WriteArrow(int sourceId, int destinationId, string? text, LineStyle lineStyle = LineStyle.Normal) =>
        WriteLink(sourceId, destinationId, GetArrowSymbol(lineStyle), text);

    private static string GetArrowSymbol(LineStyle lineStyle) => lineStyle switch
    {
        LineStyle.Normal => "-->",
        LineStyle.Dotted => "-.->",
        LineStyle.Thick => "==>",
        _ => throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null)
    };

    private void WriteLink(int sourceId, int destinationId, string linkSymbol, string? text) =>
        WriteLineIndented(text is null
            ? $"{sourceId}{linkSymbol}{destinationId}"
            : $"{sourceId}{linkSymbol}|{text}|{destinationId}");

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
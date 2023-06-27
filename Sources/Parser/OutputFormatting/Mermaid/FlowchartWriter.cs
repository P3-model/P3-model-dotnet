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
    private readonly List<string> _linksToHide = new();
    private int _shapeId;
    private int _linkId;
    private int _indent;

    public FlowchartWriter(StreamWriter writer) => _writer = writer;

    public void WriteDiagram(FlowchartDirection direction, Action<FlowchartElementsWriter> addElements)
    {
        WriteLineIndented("```mermaid");
        _indent = 1;
        WriteLineIndented($"flowchart {direction}");
        _indent = 2;
        addElements(this);
        WriteStyles();
        _indent = 0;
        WriteLineIndented("```");
    }

    private void WriteStyles()
    {
        if (_linksToHide.Count > 0)
            WriteLineIndented($"linkStyle {string.Join(',', _linksToHide)} stroke:none");
        foreach (var (name, def) in StyleDefinitions)
            WriteLineIndented($"classDef {name} {def}");
    }

    public string WriteRectangle(string name, Style style = Style.Default) =>
        WriteShape($"({name})", style);

    public string WriteStadiumShape(string name, Style style = Style.Default) =>
        WriteShape($"([{name}])", style);

    public string WriteHexagon(string name, Style style = Style.Default) =>
        WriteShape($"{{{{{name}}}}}", style);

    public string WriteCylinder(string name, Style style = Style.Default) =>
        WriteShape($"[({name})]", style);

    public string WriteCircle(string name, Style style = Style.Default) =>
        WriteShape($"(({name}))", style);

    public string WriteRhombus(string name, Style style = Style.Default) =>
        WriteShape($"{{{name}}}", style);

    private string WriteShape(string value, Style style)
    {
        var id = GetNextShapeId();
        WriteLineIndented($"{id}{value}");
        if (style != Style.Default)
            WriteLineIndented($"class {id} {style}");
        return id;
    }
    
    public void WriteOpenLink(string sourceId, string destinationId, LineStyle lineStyle) =>
        WriteOpenLink(sourceId, destinationId, null, lineStyle);

    public void WriteOpenLink(string sourceId, string destinationId, string? text = null,
        LineStyle lineStyle = LineStyle.Normal) =>
        WriteLink(sourceId, destinationId, GetOpenLinkSymbol(lineStyle), text);

    public void WriteArrow(string sourceId, string destinationId, LineStyle lineStyle) =>
        WriteArrow(sourceId, destinationId, null, lineStyle);

    public void WriteArrow(string sourceId, string destinationId, string? text,
        LineStyle lineStyle = LineStyle.Normal) =>
        WriteLink(sourceId, destinationId, GetArrowSymbol(lineStyle), text);

    public void WriteBackwardArrow(string sourceId, string destinationId, LineStyle lineStyle) =>
        WriteBackwardArrow(sourceId, destinationId, null, lineStyle);

    public void WriteBackwardArrow(string sourceId, string destinationId, string? text,
        LineStyle lineStyle = LineStyle.Normal)
    {
        WriteInvisibleLink(sourceId, destinationId);
        WriteLink(sourceId, destinationId, GetArrowSymbol(lineStyle), text);
        WriteInvisibleLink(destinationId, sourceId);
    }

    private static string GetArrowSymbol(LineStyle lineStyle) => lineStyle switch
    {
        LineStyle.Normal => "-->",
        LineStyle.Dotted => "-.->",
        LineStyle.Thick => "==>",
        _ => throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null)
    };
    
    public void WriteInvisibleLink(string sourceId, string destinationId)
    {
        var id = WriteLink(sourceId, destinationId, GetOpenLinkSymbol(LineStyle.Normal), null);
        _linksToHide.Add(id);
    }

    private static string GetOpenLinkSymbol(LineStyle lineStyle) => lineStyle switch
    {
        LineStyle.Normal => "---",
        LineStyle.Dotted => "-.-",
        LineStyle.Thick => "===",
        _ => throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null)
    };

    private string WriteLink(string sourceId, string destinationId, string linkSymbol, string? text)
    {
        var id = GetNextLinkId();
        WriteLineIndented(text is null
            ? $"{sourceId}{linkSymbol}{destinationId}"
            : $"{sourceId}{linkSymbol}|{text}|{destinationId}");
        return id;
    }

    private string WriteBackwardLink(string sourceId, string destinationId, string linkSymbol, string? text)
    {
        var id = GetNextLinkId();
        WriteLineIndented(text is null
            ? $"{sourceId}{linkSymbol}{destinationId}"
            : $"{sourceId}{linkSymbol}|{text}|{destinationId}");
        return id;
    }

    public string WriteSubgraph(string title, Action<FlowchartElementsWriter> writeElements) =>
        WriteSubgraph(title, null, writeElements);

    public string WriteSubgraph(string title, FlowchartDirection direction,
        Action<FlowchartElementsWriter> writeElements) =>
        WriteSubgraph(title, (FlowchartDirection?)direction, writeElements);

    private string WriteSubgraph(string title, FlowchartDirection? direction,
        Action<FlowchartElementsWriter> writeElements)
    {
        var id = GetNextShapeId();
        WriteLineIndented(@$"subgraph {id}[""{title}""]");
        _indent++;
        if (direction.HasValue)
            WriteLineIndented($"direction {direction.Value}");
        writeElements(this);
        _indent--;
        WriteLineIndented("end");
        return id;
    }

    private void WriteLineIndented(string value)
    {
        _writer.Write(new string(' ', _indent * 2));
        _writer.WriteLine(value);
    }

    private string GetNextShapeId() => (_shapeId++).ToString();
    private string GetNextLinkId() => (_linkId++).ToString();
}
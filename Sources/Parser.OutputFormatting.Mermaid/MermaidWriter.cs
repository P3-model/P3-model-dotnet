using System.Text;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MermaidWriter : IAsyncDisposable
{
    private readonly StreamWriter _streamWriter;

    public MermaidWriter(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (directory != null)
            Directory.CreateDirectory(directory);
        _streamWriter = new StreamWriter(path,
            Encoding.UTF8,
            new FileStreamOptions
            {
                Access = FileAccess.ReadWrite,
                Mode = FileMode.Create,
                Share = FileShare.None
            });
    }

    [PublicAPI]
    public void WriteInline(string text) => _streamWriter.Write(text);

    [PublicAPI]
    public void WriteLine(string text) => _streamWriter.WriteLine($"{text.TrimEnd()}  ");

    [PublicAPI]
    public void WriteLineBreak() => _streamWriter.WriteLine();

    [PublicAPI]
    public void WriteHorizontalRule()
    {
        _streamWriter.WriteLine();
        _streamWriter.WriteLine("---");
        _streamWriter.WriteLine();
    }

    [PublicAPI]
    public void WriteHeading(string text, int level)
    {
        _streamWriter.WriteLine();
        _streamWriter.WriteLine($"{new string('#', level)} {text}");
        _streamWriter.WriteLine();
    }

    [PublicAPI]
    public void WriteParagraph(string text)
    {
        _streamWriter.WriteLine(text);
        _streamWriter.WriteLine();
    }

    [PublicAPI]
    public void WriteLinkInline(string text, string url) => _streamWriter.Write(FormatLink(text, url));

    [PublicAPI]
    public static string FormatLink(string text, string url) => $"[{text}]({url})";

    [PublicAPI]
    public void WriteOrderedList(IEnumerable<string> values)
    {
        var no = 1;
        foreach (var value in values)
            _streamWriter.WriteLine($"{no++}. {value}");
    }

    [PublicAPI]
    public void WriteOrderedList<T>(IEnumerable<T> items, Func<T, string> valueFactory)
    {
        var no = 1;
        foreach (var item in items)
            _streamWriter.WriteLine($"{no++}. {valueFactory(item)}");
    }

    [PublicAPI]
    public void WriteUnorderedList(IEnumerable<string> values)
    {
        foreach (var value in values)
            _streamWriter.WriteLine($"- {value}");
    }

    [PublicAPI]
    public void WriteUnorderedList<T>(IEnumerable<T> items, Func<T, string> valueFactory)
    {
        foreach (var item in items)
            _streamWriter.WriteLine($"- {valueFactory(item)}");
    }
    
    [PublicAPI]
    public void WriteUnorderedList<T>(IEnumerable<T> items, Func<T, (string Value, int Level)> valueFactory)
    {
        foreach (var item in items)
        {
            var (value, level) = valueFactory(item);
            level = level < 1 ? 1 : level;
            _streamWriter.Write(new string(' ', 2 * (level - 1)));
            _streamWriter.WriteLine($"- {value}");
        }
    }

    [PublicAPI]
    public void WriteFlowchart(Action<FlowchartElementsWriter> writeElements) =>
        WriteFlowchart(FlowchartDirection.TB, writeElements);
    
    [PublicAPI]
    public void WriteFlowchart(FlowchartDirection direction, Action<FlowchartElementsWriter> writeElements)
    {
        var flowchartWriter = new FlowchartWriter(_streamWriter);
        flowchartWriter.WriteDiagram(direction, writeElements);
    }

    public async ValueTask DisposeAsync()
    {
        await _streamWriter.FlushAsync();
        await _streamWriter.DisposeAsync();
    }
}
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MermaidWriter : IAsyncDisposable
{
    private readonly StreamWriter _streamWriter;

    public MermaidWriter(string outputDirectory, string relativeFilePath)
    {
        if (!relativeFilePath.EndsWith(".md"))
            relativeFilePath = $"{relativeFilePath}.md";
        var path = Path.Combine(outputDirectory, relativeFilePath);
        _streamWriter = new StreamWriter(path, Encoding.UTF8, new FileStreamOptions
        {
            Access = FileAccess.ReadWrite,
            Mode = FileMode.Create,
            Share = FileShare.None
        });
    }

    [PublicAPI]
    public void WriteHeading(string text, int level) => _streamWriter.WriteLine($"{new string('#', level)} {text}");
    
    [PublicAPI]
    public void WriteText(string text) => _streamWriter.Write(text);

    [PublicAPI]
    public void WriteLink(string text, string url) => _streamWriter.Write($"[{text}]({url})");
    
    [PublicAPI]
    public void WriteEmptyLine() => _streamWriter.WriteLine();
    

    [PublicAPI]
    public void WriteFlowchart(Action<FlowchartElementsWriter> writeElements)
    {
        var flowchartWriter = new FlowchartWriter(_streamWriter);
        flowchartWriter.WriteDiagram(writeElements);
    }

    public async ValueTask DisposeAsync()
    {
        await _streamWriter.FlushAsync();
        await _streamWriter.DisposeAsync();
    }
}
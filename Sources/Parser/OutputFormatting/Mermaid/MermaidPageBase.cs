using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public abstract class MermaidPageBase : MermaidPage
{
    private readonly string _outputDirectory;

    public abstract string RelativeFilePath { get; }
    public abstract Element MainElement { get; }
    
    protected MermaidPageBase(string outputDirectory) => _outputDirectory = outputDirectory;

    public abstract void LinkWith(IReadOnlyCollection<MermaidPage> otherPages);

    public async Task WriteToFile()
    {
        var path = Path.Combine(_outputDirectory, RelativeFilePath.EndsWith(".md") 
            ? RelativeFilePath 
            : $"{RelativeFilePath}.md");
        await using var mermaidWriter = new MermaidWriter(path);
        WriteTo(mermaidWriter);
    }

    protected abstract void WriteTo(MermaidWriter mermaidWriter);
}
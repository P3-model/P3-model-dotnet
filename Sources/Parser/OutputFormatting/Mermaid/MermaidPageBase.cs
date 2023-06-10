using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public abstract class MermaidPageBase : MermaidPage
{
    private readonly string _outputDirectory;
    private IReadOnlyCollection<MermaidPage>? _zoomInPages;
    private IReadOnlyCollection<MermaidPage>? _zoomOutPages;
    private IReadOnlyCollection<MermaidPage>? _changePerspectivePages;

    public abstract string Header { get; }
    protected virtual string? Description => null;
    public virtual string LinkText => Header;
    public abstract string RelativeFilePath { get; }
    public abstract Element MainElement { get; }

    protected MermaidPageBase(string outputDirectory) => _outputDirectory = outputDirectory;

    public void LinkWith(IReadOnlyCollection<MermaidPage> otherPages)
    {
        _zoomInPages = otherPages.Where(IncludeInZoomInPages).ToList().AsReadOnly();
        _zoomOutPages = otherPages.Where(IncludeInZoomOutPages).ToList().AsReadOnly();
        _changePerspectivePages = otherPages.Where(IncludeInChangePerspectivePages).ToList().AsReadOnly();
    }

    protected abstract bool IncludeInZoomInPages(MermaidPage page);
    protected abstract bool IncludeInZoomOutPages(MermaidPage page);
    protected abstract bool IncludeInChangePerspectivePages(MermaidPage page);

    public async Task WriteToFile()
    {
        var path = Path.Combine(_outputDirectory, RelativeFilePath.EndsWith(".md")
            ? RelativeFilePath
            : $"{RelativeFilePath}.md");
        await using var mermaidWriter = new MermaidWriter(path);
        WriteHeader(mermaidWriter);
        WriteBody(mermaidWriter);
        WriteLinks(mermaidWriter);
        WriteFooter(mermaidWriter);
    }

    private void WriteHeader(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading(Header, 1);
        if (Description != null)
            mermaidWriter.WriteLine(Description);
    }

    protected abstract void WriteBody(MermaidWriter mermaidWriter);

    private void WriteLinks(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Next steps", 2);
        if (_zoomInPages is { Count: > 0 })
        {
            mermaidWriter.WriteHeading("Zoom-in", 3);
            mermaidWriter.WriteUnorderedList(_zoomInPages,
                p => MermaidWriter.FormatLink(p.LinkText, GetRelativePathFor(p)));
        }

        if (_zoomOutPages is { Count: > 0 })
        {
            mermaidWriter.WriteHeading("Zoom-out", 3);
            mermaidWriter.WriteUnorderedList(_zoomOutPages,
                p => MermaidWriter.FormatLink(p.LinkText, GetRelativePathFor(p)));
        }
        
        if (_changePerspectivePages is { Count: > 0 })
        {
            mermaidWriter.WriteHeading("Change perspective", 3);
            mermaidWriter.WriteUnorderedList(_changePerspectivePages,
                p => MermaidWriter.FormatLink(p.LinkText, GetRelativePathFor(p)));
        }
    }

    private string GetRelativePathFor(MermaidPage linkedPage)
    {
        var currentPageDirectory = Path.GetDirectoryName(RelativeFilePath);
        return string.IsNullOrEmpty(currentPageDirectory)
            ? linkedPage.RelativeFilePath 
            : Path.GetRelativePath(currentPageDirectory, linkedPage.RelativeFilePath);
    }

    private static void WriteFooter(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHorizontalRule();
        mermaidWriter.WriteLinkInline("P3 Model", "https://github.com/P3-model/P3-model");
        mermaidWriter.WriteInline(" documentation generated from source code using ");
        mermaidWriter.WriteLinkInline(".net tooling", "https://github.com/P3-model/P3-model-dotnet");
    }
}
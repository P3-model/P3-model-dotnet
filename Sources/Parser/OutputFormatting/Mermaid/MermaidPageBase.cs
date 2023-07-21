using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public abstract class MermaidPageBase : MermaidPage
{
    private readonly string _outputDirectory;
    private IReadOnlyCollection<PerspectiveLinks>? _zoomInLinks;
    private IReadOnlyCollection<PerspectiveLinks>? _zoomOutLinks;

    public virtual string Header => MainElement?.Name
                                    ?? throw new InvalidOperationException(
                                        "Page without main element must have header defined.");

    protected virtual string? Description => null;
    public virtual string LinkText => MainElement is null ? Header : MainElement.Name;
    public abstract string RelativeFilePath { get; }
    public abstract Element? MainElement { get; }
    public virtual Perspective? Perspective => MainElement?.Perspective;

    protected MermaidPageBase(string outputDirectory) => _outputDirectory = outputDirectory;

    public void LinkWith(IReadOnlyCollection<MermaidPage> otherPages)
    {
        _zoomInLinks = CreateLinks(otherPages, IncludeInZoomInPages);;
        _zoomOutLinks = CreateLinks(otherPages, IncludeInZoomOutPages);
    }

    private static ReadOnlyCollection<PerspectiveLinks> CreateLinks(IEnumerable<MermaidPage> otherPages,
        Func<MermaidPage, bool> predicate) => otherPages
        .Where(predicate)
        .GroupBy(p => p.Perspective)
        .Select(g => new PerspectiveLinks(g.Key, g
            .GroupBy(p => p.MainElement?.GetType())
            .Select(g2 => new ElementTypeLinks(g2.Key, g2
                .GroupBy(p => p.MainElement)
                .Select(g3 => new ElementLinks(g3.Key, g3
                    .OrderBy(p => p.LinkText)
                    .ToList()))
                .OrderBy(l => l.Element?.Name)
                .ToList()))
            .OrderBy(l => l.ElementType?.Name)
            .ToList()))
        .OrderBy(l => l.Perspective)
        .ToList()
        .AsReadOnly();

    protected abstract bool IncludeInZoomInPages(MermaidPage page);
    protected abstract bool IncludeInZoomOutPages(MermaidPage page);

    public async Task WriteToFile()
    {
        var path = GetAbsolutePath(NormalizeFilePath(RelativeFilePath));
        await using var mermaidWriter = new MermaidWriter(path);
        WriteHeader(mermaidWriter);
        WriteBody(mermaidWriter);
        WriteLinks(mermaidWriter);
        WriteFooter(mermaidWriter);
    }

    protected string GetPathRelativeToPageFile(string path)
    {
        var filePath = GetAbsolutePath(NormalizeFilePath(RelativeFilePath));
        var fileInfo = new FileInfo(filePath);
        var directoryPath = fileInfo.Directory != null
            ? fileInfo.Directory.FullName
            : string.Empty;
        return Path.GetRelativePath(directoryPath, path);
    }

    protected string GetAbsolutePath(string pathRelativeToOutputDirectory) =>
        Path.Combine(_outputDirectory, pathRelativeToOutputDirectory);

    private static string NormalizeFilePath(string path) => path.EndsWith(".md") ? path : $"{path}.md";

    private void WriteHeader(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading(Header, 1);
        if (Description == null)
            return;
        mermaidWriter.WriteLine(Description);
        mermaidWriter.WriteHorizontalRule();
        mermaidWriter.WriteLineBreak();
    }

    protected abstract void WriteBody(MermaidWriter mermaidWriter);

    private void WriteLinks(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Next steps", 2);
        if (_zoomInLinks is { Count: > 0 })
        {
            mermaidWriter.WriteHeading("Zoom-in", 3);
            WriteLinks(mermaidWriter, _zoomInLinks);
        }

        if (_zoomOutLinks is { Count: > 0 })
        {
            mermaidWriter.WriteHeading("Zoom-out", 3);
            WriteLinks(mermaidWriter, _zoomOutLinks);
        }
    }

    private void WriteLinks(MermaidWriter mermaidWriter, IEnumerable<PerspectiveLinks> links)
    {
        foreach (var perspectiveLinks in links)
        {
            var perspective = perspectiveLinks.Perspective;
            var perspectiveHeading = perspective.HasValue ? $"{perspective} perspective" : "Multi perspectives";
            mermaidWriter.WriteHeading(perspectiveHeading, 4);
            foreach (var elementTypeLinks in perspectiveLinks.Links)
            {
                mermaidWriter.WriteHeading(elementTypeLinks.ElementType?.Name.Humanize().Pluralize()
                                           ?? "Cross elements", 5);
                foreach (var elementLinks in elementTypeLinks.Links)
                {
                    if (elementLinks.Pages.Count == 1)
                    {
                        var page = elementLinks.Pages[0];
                        mermaidWriter.WriteLine(MermaidWriter.FormatLink(page.LinkText,
                            GetRelativePathFor(page)));
                    }
                    else
                    {
                        if (elementLinks.Element != null)
                            mermaidWriter.WriteHeading(elementLinks.Element.Name, 6);
                        foreach (var page in elementLinks.Pages)
                            mermaidWriter.WriteLine(MermaidWriter.FormatLink(page.LinkText,
                                GetRelativePathFor(page)));
                    }
                }
            }
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

    private record PerspectiveLinks(Perspective? Perspective, IReadOnlyList<ElementTypeLinks> Links);

    private record ElementTypeLinks(Type? ElementType, IReadOnlyList<ElementLinks> Links);

    private record ElementLinks(Element? Element, IReadOnlyList<MermaidPage> Pages);
}
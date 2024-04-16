using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public interface MermaidPage
{
    string Header { get; }
    string LinkText { get; }
    string RelativeFilePath { get; }
    ElementBase? MainElement { get; }
    Perspective? Perspective { get; }

    void LinkWith(IReadOnlyCollection<MermaidPage> otherPages);
    Task WriteToFile();
}
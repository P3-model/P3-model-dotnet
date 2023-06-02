using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public interface MermaidPage
{
    string RelativeFilePath { get; }
    void Write(Model model, MermaidWriter mermaidWriter, MermaidPages allPages);
}
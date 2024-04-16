using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public interface MermaidPageFactory
{
    IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph);
}
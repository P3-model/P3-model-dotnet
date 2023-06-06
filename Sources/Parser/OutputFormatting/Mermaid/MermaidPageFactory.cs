using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public interface MermaidPageFactory
{
    IEnumerable<MermaidPage> Create(string outputDirectory, Model model);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public interface MermaidPage
{
    string RelativeFilePath { get; }
    Element MainElement { get; }
    
    void LinkWith(IReadOnlyCollection<MermaidPage> otherPages);
    Task WriteToFile();
}
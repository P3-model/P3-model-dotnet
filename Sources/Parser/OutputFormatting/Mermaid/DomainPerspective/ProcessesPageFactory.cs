using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class ProcessesPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new ProcessesPage(outputDirectory,
            model.Cache.Product,
            model.Cache.ProcessesHierarchy);
    }
}
using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainModulesPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new DomainModulesPage(outputDirectory,
            model.Cache.Product,
            model.Cache.DomainModulesHierarchy);
    }
}
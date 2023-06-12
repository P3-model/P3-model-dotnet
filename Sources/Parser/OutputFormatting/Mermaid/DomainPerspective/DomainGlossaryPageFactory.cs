using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainGlossaryPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new DomainGlossaryPage(outputDirectory,
            model.Cache.Product,
            model.Cache.DomainModulesHierarchy,
            model.Cache.DomainBuildingBlocks);
    }
}
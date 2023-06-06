using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainBuildingBlocksPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new DomainBuildingBlocksPage(outputDirectory,
            model.Cache.DomainModulesHierarchy,
            model.Cache.DomainBuildingBlocks
        );
    }
}
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainBuildingBlocksPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DomainBuildingBlocksPage(outputDirectory,
            modelGraph.Cache.DomainModulesHierarchy,
            modelGraph.Cache.DomainBuildingBlocks
        );
    }
}
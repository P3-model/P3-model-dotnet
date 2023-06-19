using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainGlossaryPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DomainGlossaryPage(outputDirectory,
            modelGraph.Execute(Query.Elements<Product>().Single()),
            modelGraph.Cache.DomainModulesHierarchy,
            modelGraph.Cache.DomainBuildingBlocks);
    }
}
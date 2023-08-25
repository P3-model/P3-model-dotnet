using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainGlossaryPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DomainGlossaryPage(outputDirectory,
            modelGraph.Execute(query => query
                .Hierarchy<DomainModule, DomainModule.ContainsDomainModule>()),
            modelGraph.Execute(query => query
                    .Relations<DomainModule.ContainsBuildingBlock>())
                .Select(r => new DomainGlossaryPage.BuildingBlockInfo(r.Destination, r.Source, modelGraph
                    .Execute(traitQuery => traitQuery
                        .Traits<DomainBuildingBlockDescription>(trait => trait.Element.Equals(r.Destination)))
                    .SingleOrDefault()))
                .ToList());
    }
}
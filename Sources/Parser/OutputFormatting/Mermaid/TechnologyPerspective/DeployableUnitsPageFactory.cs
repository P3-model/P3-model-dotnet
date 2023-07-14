using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

[UsedImplicitly]
public class DeployableUnitsPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DeployableUnitsPage(outputDirectory,
            modelGraph.System,
            modelGraph.Execute(query => query
                .AllElements<DeployableUnit>()),
            modelGraph.Execute(query => query
                .Relations<Tier.ContainsDeployableUnit>()));
    }
}
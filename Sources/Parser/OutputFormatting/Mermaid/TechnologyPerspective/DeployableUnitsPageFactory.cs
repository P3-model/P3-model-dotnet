using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

[UsedImplicitly]
public class DeployableUnitsPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DeployableUnitsPage(outputDirectory,
            modelGraph.Execute(Query
                .Elements<Product>()
                .Single()),
            modelGraph.Execute(Query
                .Elements<DeployableUnit>()
                .All()),
            modelGraph.Execute(Query
                .Relations<Tier.ContainsDeployableUnit>()));
    }
}
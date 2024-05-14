using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Technology;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.Technology;

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
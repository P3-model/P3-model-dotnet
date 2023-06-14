using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

[UsedImplicitly]
public class DeployableUnitsPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new DeployableUnitsPage(outputDirectory,
            model.Cache.Product,
            model.Elements.OfType<DeployableUnit>(),
            model.Relations.OfType<Tier.ContainsDeployableUnit>());
    }
}
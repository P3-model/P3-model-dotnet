using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

[UsedImplicitly]
public class DeployableUnitPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        return model.Elements
            .OfType<DeployableUnit>()
            .Select(unit =>
            {
                var tier = model.Relations
                    .OfType<Tier.ContainsDeployableUnit>()
                    .SingleOrDefault(r => r.DeployableUnit.Equals(unit))
                    ?.Tier;
                var domainModules = new HashSet<DomainModule>(model.Relations
                    .OfType<DomainModule.IsDeployedInDeployableUnit>()
                    .Where(r => r.DeployableUnit.Equals(unit))
                    .Select(r => r.DomainModule)
                    .Where(m => m.Level == 0));
                var teams = model.Relations
                    .OfType<DevelopmentTeam.OwnsDomainModule>()
                    .Where(r => domainModules.Contains(r.DomainModule))
                    .Select(r => r.Team)
                    .Distinct();
                return new DeployableUnitPage(outputDirectory, unit, tier, domainModules, teams);
            });
    }
}
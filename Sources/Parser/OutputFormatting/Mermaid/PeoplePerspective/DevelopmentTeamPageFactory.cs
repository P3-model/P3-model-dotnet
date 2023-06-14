using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class DevelopmentTeamPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        return model.Elements
            .OfType<DevelopmentTeam>()
            .Select(team =>
            {
                var domainModules = model.Relations
                    .OfType<DevelopmentTeam.OwnsDomainModule>()
                    .Where(r => r.Team.Equals(team))
                    .Select(r => r.DomainModule)
                    .Distinct();
                var deployableUnits = model.Relations
                    .OfType<DevelopmentTeam.OwnsDomainModule>()
                    .Where(r => r.Team.Equals(team))
                    .Select(r => r.DomainModule)
                    .SelectMany(m => model.Relations
                        .OfType<DomainModule.IsDeployedInDeployableUnit>()
                        .Where(r2 => r2.DomainModule.Equals(m))
                        .Select(r2 => r2.DeployableUnit))
                    .Distinct();
                return new DevelopmentTeamPage(outputDirectory, team, domainModules, deployableUnits);
            });
    }
}
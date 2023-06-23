using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

[UsedImplicitly]
public class DeployableUnitPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<DeployableUnit>())
        .Select(unit =>
        {
            var tier = modelGraph.Execute(query => query
                    .Elements<Tier>()
                    .RelatedTo(unit)
                    .ByRelation<Tier.ContainsDeployableUnit>())
                .SingleOrDefault();
            var domainModules = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo(unit)
                    .ByRelation<DomainModule.IsDeployedInDeployableUnit>())
                .Where(m => m.Level == 0)
                .ToHashSet();
            var teams = modelGraph.Execute(query => query
                .Elements<DevelopmentTeam>()
                .RelatedToAny(domainModules)
                .ByRelation<DevelopmentTeam.OwnsDomainModule>());
            return new DeployableUnitPage(outputDirectory, unit, tier, domainModules, teams);
        });
}
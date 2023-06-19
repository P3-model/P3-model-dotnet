using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class DevelopmentTeamPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(Query
            .Elements<DevelopmentTeam>()
            .All())
        .Select(team =>
        {
            var domainModules = modelGraph.Execute(Query
                .Elements<DomainModule>()
                .RelatedTo(team)
                .ByReverseRelation<DevelopmentTeam.OwnsDomainModule>());
            var deployableUnits = modelGraph.Execute(Query
                .Elements<DeployableUnit>()
                .RelatedTo(domainModules)
                .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());;
            return new DevelopmentTeamPage(outputDirectory, team, domainModules, deployableUnits);
        });
}
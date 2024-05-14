using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.People;

[UsedImplicitly]
public class DevelopmentTeamPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<DevelopmentTeam>())
        .Select(team =>
        {
            var domainModules = modelGraph.Execute(query => query
                .Elements<DomainModule>()
                .RelatedTo(team)
                .ByReverseRelation<DevelopmentTeam.OwnsDomainModule>());
            var deployableUnits = modelGraph.Execute(query => query
                .Elements<DeployableUnit>()
                .RelatedToAny(domainModules)
                .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());
            return new DevelopmentTeamPage(outputDirectory, team, domainModules, deployableUnits);
        });
}
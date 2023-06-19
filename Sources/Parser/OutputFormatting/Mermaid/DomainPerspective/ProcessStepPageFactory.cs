using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class ProcessStepPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) =>
        modelGraph.Execute(Query
                .Elements<ProcessStep>()
                .All())
            .Select(step =>
            {
                var process = modelGraph.Execute(Query
                        .Elements<Process>()
                        .RelatedTo(step)
                        .ByRelation<Process.ContainsProcessStep>())
                    // TODO: unique names across hierarchy
                    .FirstOrDefault();
                var domainModule = modelGraph.Execute(Query
                        .Elements<DomainModule>()
                        .RelatedTo(step)
                        .ByReverseRelation<ProcessStep.BelongsToDomainModule>())
                    .SingleOrDefault(m => m.Level == 0);
                var deployableUnit = domainModule is null
                    ? null
                    : modelGraph.Execute(Query
                            .Elements<DeployableUnit>()
                            .RelatedTo(domainModule)
                            .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>())
                        .SingleOrDefault();
                var actors = modelGraph.Execute(Query
                    .Elements<Actor>()
                    .RelatedTo(step)
                    .ByRelation<Actor.UsesProcessStep>());
                var developmentTeams = domainModule is null
                    ? Enumerable.Empty<DevelopmentTeam>()
                    : modelGraph.Execute(Query
                        .Elements<DevelopmentTeam>()
                        .RelatedTo(domainModule)
                        .ByRelation<DevelopmentTeam.OwnsDomainModule>());
                var organizationalUnits = domainModule is null
                    ? Enumerable.Empty<BusinessOrganizationalUnit>()
                    : modelGraph.Execute(Query
                        .Elements<BusinessOrganizationalUnit>()
                        .RelatedTo(domainModule)
                        .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
                return new ProcessStepPage(outputDirectory, step, process, domainModule, deployableUnit, actors,
                    developmentTeams, organizationalUnits);
            });
}
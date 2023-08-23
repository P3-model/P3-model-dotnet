using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class ProcessStepPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<ProcessStep>())
        .Select(step =>
        {
            var process = modelGraph.Execute(query => query
                    .Elements<Process>()
                    .RelatedTo(step)
                    .ByRelation<Process.ContainsProcessStep>())
                .FirstOrDefault();
            // TODO: warning logging if more than one element (non unique names o process steps)
            var module = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo(step)
                    .ByRelation<DomainModule.ContainsProcessStep>())
                .FirstOrDefault();
            var buildingBlocks = modelGraph.Execute(query => query
                .Elements<DomainBuildingBlock>()
                .RelatedTo(step)
                .ByReverseRelation<ProcessStep.DependsOnBuildingBlock>());
            var deployableUnit = module is null
                ? null
                : modelGraph.Execute(query => query
                        .Elements<DeployableUnit>()
                        .RelatedTo(module)
                        .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>())
                    // TODO: Identifying deployable units by technology relationship (ModelBoundary can by deployed in more than one unit).
                    .FirstOrDefault();
            var actors = modelGraph.Execute(query => query
                .Elements<Actor>()
                .RelatedTo(step)
                .ByRelation<Actor.UsesProcessStep>());
            var developmentTeams = module is null
                ? new HashSet<DevelopmentTeam>()
                : modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedTo(module)
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>());
            var organizationalUnits = module is null
                ? new HashSet<BusinessOrganizationalUnit>()
                : modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedTo(module)
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
            return new ProcessStepPage(outputDirectory, step, process, buildingBlocks, deployableUnit, actors,
                developmentTeams, organizationalUnits);
        });
}
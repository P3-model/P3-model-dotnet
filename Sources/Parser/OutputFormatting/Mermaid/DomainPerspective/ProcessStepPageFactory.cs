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
                .SingleOrDefault();
            // TODO: warning logging if more than one element (non unique names o process steps)
            var modelBoundary = modelGraph.Execute(query => query
                    .Elements<ModelBoundary>()
                    .RelatedTo(step)
                    .ByRelation<ModelBoundary.ContainsProcessStep>())
                .FirstOrDefault();
            var buildingBlocks = modelGraph.Execute(query => query
                .Elements<DomainBuildingBlock>()
                .RelatedTo(step)
                .ByReverseRelation<ProcessStep.DependsOnBuildingBlock>());
            var deployableUnit = modelBoundary is null
                ? null
                : modelGraph.Execute(query => query
                        .Elements<DeployableUnit>()
                        .RelatedTo(modelBoundary)
                        .ByReverseRelation<ModelBoundary.IsDeployedInDeployableUnit>())
                    // TODO: Identifying deployable units by technology relationship (ModelBoundary can by deployed in more than one unit).
                    .FirstOrDefault();
            var actors = modelGraph.Execute(query => query
                .Elements<Actor>()
                .RelatedTo(step)
                .ByRelation<Actor.UsesProcessStep>());
            var developmentTeams = modelBoundary is null
                ? new HashSet<DevelopmentTeam>()
                : modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedTo(modelBoundary)
                    .ByRelation<DevelopmentTeam.OwnsModelBoundary>());
            var organizationalUnits = modelBoundary is null
                ? new HashSet<BusinessOrganizationalUnit>()
                : modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedTo(modelBoundary)
                    .ByRelation<BusinessOrganizationalUnit.OwnsModelBoundary>());
            return new ProcessStepPage(outputDirectory, step, process, buildingBlocks, deployableUnit, actors,
                developmentTeams, organizationalUnits);
        });
}
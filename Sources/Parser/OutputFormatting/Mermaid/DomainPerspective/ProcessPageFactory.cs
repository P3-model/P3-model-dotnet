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
public class ProcessPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        var modulesHierarchy = modelGraph.HierarchyFor<DomainModule, DomainModule.ContainsDomainModule>();
        return modelGraph
            .Execute(query => query
                .AllElements<Process>())
            .Select(process =>
            {
                var parent = modelGraph.Execute(query => query
                        .Elements<Process>()
                        .RelatedTo(process)
                        .ByRelation<Process.ContainsSubProcess>())
                    .SingleOrDefault();
                var children = modelGraph.Execute(query => query
                    .Elements<Process>()
                    .RelatedTo(process)
                    .ByReverseRelation<Process.ContainsSubProcess>());
                var processHasNextSubProcessRelations = modelGraph.Execute(query => query
                    .Relations<Process.HasNextSubProcess>(r => children.Contains(r.Source) &&
                                                               children.Contains(r.Destination)));
                var allSteps = modelGraph.Execute(query => query
                    .Elements<ProcessStep>()
                    .RelatedToAny(subQuery => subQuery
                        .DescendantsAndSelf<Process, Process.ContainsSubProcess>(process))
                    .ByReverseRelation<Process.ContainsProcessStep>());
                var directSteps = modelGraph.Execute(query => query
                    .Elements<ProcessStep>()
                    .RelatedTo(process)
                    .ByReverseRelation<Process.ContainsProcessStep>());
                var modelBoundaries = modelGraph.Execute(query => query
                    .Elements<ModelBoundary>()
                    .RelatedToAny(allSteps)
                    .ByRelation<ModelBoundary.ContainsProcessStep>());
                var deployableUnits = modelGraph.Execute(query => query
                    .Elements<DeployableUnit>()
                    .RelatedToAny(modelBoundaries)
                    .ByReverseRelation<ModelBoundary.IsDeployedInDeployableUnit>());
                var actors = modelGraph.Execute(query => query
                    .Elements<Actor>()
                    .RelatedToAny(allSteps)
                    .ByRelation<Actor.UsesProcessStep>());
                var developmentTeams = modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedToAny(modelBoundaries)
                    .ByRelation<DevelopmentTeam.OwnsModelBoundary>());
                var organizationalUnits = modelGraph.Execute(query => query
                        .Elements<BusinessOrganizationalUnit>()
                        .RelatedToAny(modelBoundaries)
                        .ByRelation<BusinessOrganizationalUnit.OwnsModelBoundary>());
                return new ProcessPage(outputDirectory, process, parent, children, processHasNextSubProcessRelations,
                    directSteps, modelBoundaries, deployableUnits!, actors, developmentTeams,
                    organizationalUnits);
            });
    }
}
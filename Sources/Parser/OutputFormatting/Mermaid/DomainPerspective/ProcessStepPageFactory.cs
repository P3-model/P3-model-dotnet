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
            var modules = modelGraph.Execute(query => query
                .Elements<DomainModule>()
                .RelatedTo(step)
                .ByReverseRelation<ProcessStep.BelongsToDomainModule>());
            // TODO: warning logging if more than one element (non unique names o process steps)
            var module = modules.FirstOrDefault();
            var deployableUnit = module is null
                ? null
                : modelGraph.Execute(query => query
                    .Elements<DeployableUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>(filter => filter
                        .MaxBy(r => r.Source.Level)));
            var actors = modelGraph.Execute(query => query
                .Elements<Actor>()
                .RelatedTo(step)
                .ByRelation<Actor.UsesProcessStep>());
            var developmentTeams = module is null
                ? Enumerable.Empty<DevelopmentTeam>()
                : modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>(filter => filter
                        .GroupBy(r => r.Destination.Level)
                        .MaxBy(g => g.Key)!));
            var organizationalUnits = module is null
                ? Enumerable.Empty<BusinessOrganizationalUnit>()
                : modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>(filter => filter
                        .GroupBy(r => r.Destination.Level)
                        .MaxBy(g => g.Key)!));
            return new ProcessStepPage(outputDirectory, step, process, module, deployableUnit, actors,
                developmentTeams, organizationalUnits);
        });
}
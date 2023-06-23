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
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
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
                .ByReverseRelation<Process.ContainsProcessStep>()
                .GetAll());
            var directSteps = modelGraph.Execute(query => query
                .Elements<ProcessStep>()
                .RelatedTo(process)
                .ByReverseRelation<Process.ContainsProcessStep>());
            var domainModules = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedToAny(allSteps)
                    .ByReverseRelation<ProcessStep.BelongsToDomainModule>()
                    .GetAll())
                .Where(m => m.Level == 0)
                .ToHashSet();
            var deployableUnits = modelGraph.Execute(query => query
                .Elements<DeployableUnit>()
                .RelatedToAny(domainModules)
                .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>()
                .GetAll());
            var actors = modelGraph.Execute(query => query
                .Elements<Actor>()
                .RelatedToAny(allSteps)
                .ByRelation<Actor.UsesProcessStep>()
                .GetAll());
            var developmentTeams = modelGraph.Execute(query => query
                .Elements<DevelopmentTeam>()
                .RelatedToAny(domainModules)
                .ByRelation<DevelopmentTeam.OwnsDomainModule>()
                .GetAll());
            var organizationalUnits = modelGraph.Execute(query => query
                .Elements<BusinessOrganizationalUnit>()
                .RelatedToAny(domainModules)
                .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>()
                .GetAll());
            return new ProcessPage(outputDirectory, process, parent, children, processHasNextSubProcessRelations,
                directSteps, domainModules, deployableUnits, actors, developmentTeams, organizationalUnits);
        });
}
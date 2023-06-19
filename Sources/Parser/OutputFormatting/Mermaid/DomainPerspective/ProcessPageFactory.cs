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
public class ProcessPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(Query
            .Elements<Process>()
            .All())
        .Select(process =>
        {
            var parent = modelGraph.Execute(Query
                    .Elements<Process>()
                    .RelatedTo(process)
                    .ByRelation<Process.ContainsSubProcess>())
                .SingleOrDefault();
            var children = modelGraph.Execute(Query
                .Elements<Process>()
                .RelatedTo(process)
                .ByReverseRelation<Process.ContainsSubProcess>());
            var processHasNextSubProcessRelations = modelGraph.Execute(Query
                .Relations<Process.HasNextSubProcess>(r => children.Contains(r.Source) &&
                                                           children.Contains(r.Destination)));
            var allSteps = modelGraph.Execute(Query
                .Elements<ProcessStep>()
                .RelatedTo(process)
                .ByReverseRelation<Process.ContainsProcessStep>());
            var directSteps = modelGraph.Execute(Query
                .Elements<ProcessStep>()
                .RelatedTo(process)
                .ByReverseRelation<Process.DirectlyContainsProcessStep>());
            var domainModules = modelGraph.Execute(Query
                    .Elements<DomainModule>()
                    .RelatedTo(allSteps)
                    .ByReverseRelation<ProcessStep.BelongsToDomainModule>())
                .Where(m => m.Level == 0)
                .ToHashSet();
            var deployableUnits = modelGraph.Execute(Query
                .Elements<DeployableUnit>()
                .RelatedTo(domainModules)
                .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());
            var actors = modelGraph.Execute(Query
                .Elements<Actor>()
                .RelatedTo(allSteps)
                .ByRelation<Actor.UsesProcessStep>());
            var developmentTeams = modelGraph.Execute(Query
                .Elements<DevelopmentTeam>()
                .RelatedTo(domainModules)
                .ByRelation<DevelopmentTeam.OwnsDomainModule>());
            var organizationalUnits = modelGraph.Execute(Query
                .Elements<BusinessOrganizationalUnit>()
                .RelatedTo(domainModules)
                .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
            return new ProcessPage(outputDirectory, process, parent, children, processHasNextSubProcessRelations,
                directSteps, domainModules, deployableUnits, actors, developmentTeams, organizationalUnits);
        });
}
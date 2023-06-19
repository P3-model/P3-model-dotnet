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
public class DomainModulePageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        return modelGraph.Execute(Query
                .Elements<DomainModule>()
                .All())
            .Select(module =>
            {
                var parent = modelGraph.Execute(Query
                    .Elements<DomainModule>()
                    .RelatedTo(module)
                    .ByRelation<DomainModule.ContainsDomainModule>())
                    .SingleOrDefault();
                var children = modelGraph.Execute(Query
                    .Elements<DomainModule>()
                    .RelatedTo(module)
                    .ByReverseRelation<DomainModule.ContainsDomainModule>());
                var steps = modelGraph.Execute(Query
                    .Elements<ProcessStep>()
                    .RelatedTo(module)
                    .ByRelation<ProcessStep.BelongsToDomainModule>());
                var processes = modelGraph.Execute(Query
                    .Elements<Process>()
                    .RelatedTo(steps)
                    .ByRelation<Process.ContainsProcessStep>());
                var deployableUnits = modelGraph.Execute(Query
                    .Elements<DeployableUnit>()
                    .RelatedTo(module)
                    .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());
                // TODO: relation to owners from all hierarchy levels
                var developmentTeams = modelGraph.Execute(Query
                    .Elements<DevelopmentTeam>()
                    .RelatedTo(module)
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>());
                var organizationalUnits = modelGraph.Execute(Query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedTo(module)
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
                return new DomainModulePage(outputDirectory, module, parent, children, processes, deployableUnits,
                    developmentTeams, organizationalUnits);
            });
    }
}
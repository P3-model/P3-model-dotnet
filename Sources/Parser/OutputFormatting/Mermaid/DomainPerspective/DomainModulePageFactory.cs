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
public class DomainModulePageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        return modelGraph.Execute(query => query
            .AllElements<DomainModule>())
            .Select(module =>
            {
                var parent = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo(module)
                    .ByRelation<DomainModule.ContainsDomainModule>())
                    .SingleOrDefault();
                var children = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo(module)
                    .ByReverseRelation<DomainModule.ContainsDomainModule>());
                var allBuildingBlocks = modelGraph.Execute(query => query
                    .Elements<DomainBuildingBlock>()
                    .RelatedToAny(subQuery => subQuery
                        .DescendantsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByReverseRelation<DomainModule.ContainsBuildingBlock>());
                var steps = modelGraph.Execute(query => query
                    .Elements<ProcessStep>()
                    .RelatedToAny(allBuildingBlocks)
                    .ByRelation<ProcessStep.DependsOnBuildingBlock>());
                var processes = modelGraph.Execute(query => query
                    .Elements<Process>()
                    .RelatedToAny(steps)
                    .ByRelation<Process.ContainsProcessStep>());
                var deployableUnits = modelGraph.Execute(query => query
                    .Elements<DeployableUnit>()
                    .RelatedTo(module)
                    .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());
                // TODO: relation to owners from all hierarchy levels
                var developmentTeams = modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedTo(module)
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>());
                var organizationalUnits = modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedTo(module)
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
                return new DomainModulePage(outputDirectory, module, parent, children, processes, deployableUnits,
                    developmentTeams, organizationalUnits);
            });
    }
}
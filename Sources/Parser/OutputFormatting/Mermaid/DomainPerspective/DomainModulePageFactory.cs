using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

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
                var directBuildingBlocks = modelGraph.Execute(query => query
                    .Elements<DomainBuildingBlock>()
                    .RelatedTo(module)
                    .ByReverseRelation<DomainModule.ContainsBuildingBlock>());
                var allBuildingBlocks = modelGraph.Execute(query => query
                    .Elements<DomainBuildingBlock>()
                    .RelatedToAny(subQuery => subQuery
                        .DescendantsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByReverseRelation<DomainModule.ContainsBuildingBlock>());
                var steps = allBuildingBlocks
                    .OfType<ProcessStep>()
                    .Union(modelGraph.Execute(query => query
                        .Elements<ProcessStep>()
                        .RelatedToAny(allBuildingBlocks)
                        .ByRelation<ProcessStep.DependsOnBuildingBlock>()))
                    .ToHashSet();
                var processes = modelGraph.Execute(query => query
                    .Elements<Process>()
                    .RelatedToAny(steps)
                    .ByRelation<Process.ContainsProcessStep>());
                var deployableUnits = modelGraph.GetDeployableUnitsFor(module);
                var teams = modelGraph.GetDevelopmentTeamsFor(module);
                var organizationalUnits = modelGraph.GetBusinessOrganizationalUnitsFor(module);
                return new DomainModulePage(outputDirectory, module, parent, children, processes, directBuildingBlocks,
                    deployableUnits, teams, organizationalUnits);
            });
    }
}
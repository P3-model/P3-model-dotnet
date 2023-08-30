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
                var ancestorDeployableUnit = modelGraph.Execute(query => query
                    .Elements<DeployableUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>(filter => filter
                        .MaxBy(r => r.Source.Level)));
                var descendantsDeployableUnits = modelGraph.Execute(query => query
                    .Elements<DeployableUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .Descendants<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());
                var deployableUnits = ancestorDeployableUnit != null
                    ? descendantsDeployableUnits
                        .Union(new[] { ancestorDeployableUnit })
                        .ToHashSet()
                    : descendantsDeployableUnits;
                var ancestorTeam = modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>(filter => filter
                        .MaxBy(g => g.Destination.Level)));
                var descendantsTeams = modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedToAny(subQuery => subQuery
                        .Descendants<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>());
                var teams = ancestorTeam != null
                    ? descendantsTeams
                        .Union(new[] { ancestorTeam })
                        .ToHashSet()
                    : descendantsTeams;
                var ancestorOrganizationalUnit = modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>(filter => filter
                        .MaxBy(g => g.Destination.Level)));
                var descendantsOrganizationalUnits = modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .Descendants<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
                var organizationalUnits = ancestorOrganizationalUnit != null
                    ? descendantsOrganizationalUnits
                        .Union(new[] { ancestorOrganizationalUnit })
                        .ToHashSet()
                    : descendantsOrganizationalUnits;
                return new DomainModulePage(outputDirectory, module, parent, children, processes, directBuildingBlocks,
                    deployableUnits, teams, organizationalUnits);
            });
    }
}
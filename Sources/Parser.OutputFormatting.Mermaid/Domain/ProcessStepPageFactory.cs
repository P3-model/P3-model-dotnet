using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.Domain;

[UsedImplicitly]
public class UseCasePageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<UseCase>())
        .Select(useCase =>
        {
            var process = modelGraph.Execute(query => query
                    .Elements<Process>()
                    .RelatedTo(useCase)
                    .ByRelation<Process.ContainsUseCase>())
                .SingleOrDefault();
            // TODO: warning logging if more than one element (non unique names o use cases)
            var module = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo((DomainBuildingBlock)useCase)
                    .ByRelation<DomainModule.ContainsBuildingBlock>())
                .SingleOrDefault();
            var buildingBlocks = modelGraph.Execute(query => query
                .Elements<DomainBuildingBlock>()
                .RelatedTo(useCase)
                .ByReverseRelation<UseCase.DependsOnBuildingBlock>());
            var deployableUnit = module is null
                ? null
                : modelGraph.Execute(query => query
                    .Elements<DeployableUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>(filter => filter
                        .MaxBy(r => r.Source.HierarchyPath.Level)));
            var actors = modelGraph.Execute(query => query
                .Elements<Actor>()
                .RelatedTo(useCase)
                .ByRelation<Actor.UsesUseCase>());
            var developmentTeams = module is null
                ? new HashSet<DevelopmentTeam>()
                : modelGraph.Execute(query => query
                    .Elements<DevelopmentTeam>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<DevelopmentTeam.OwnsDomainModule>(filter => filter
                        .GroupBy(r => r.Destination)
                        .MaxBy(g => g.Key.HierarchyPath.Level) ?? Enumerable.Empty<DevelopmentTeam.OwnsDomainModule>()));
            var organizationalUnits = module is null
                ? new HashSet<BusinessOrganizationalUnit>()
                : modelGraph.Execute(query => query
                    .Elements<BusinessOrganizationalUnit>()
                    .RelatedToAny(subQuery => subQuery
                        .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                    .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>(filter => filter
                        .GroupBy(r => r.Destination)
                        .MaxBy(g => g.Key.HierarchyPath.Level) ?? Enumerable.Empty<BusinessOrganizationalUnit.OwnsDomainModule>()));
            var codeStructures = modelGraph.Execute(query => query
                .Elements<CodeStructure>()
                .RelatedTo((DomainBuildingBlock)useCase)
                .ByReverseRelation<DomainBuildingBlock.IsImplementedBy>());
            return new UseCasePage(outputDirectory, useCase, module, process, buildingBlocks, deployableUnit, actors,
                developmentTeams, organizationalUnits, codeStructures);
        });
}
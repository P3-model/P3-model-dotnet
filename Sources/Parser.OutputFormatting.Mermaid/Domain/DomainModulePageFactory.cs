using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.Domain;

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
                var useCases = allBuildingBlocks
                    .OfType<UseCase>()
                    .Union(modelGraph.Execute(query => query
                        .Elements<UseCase>()
                        .RelatedToAny(allBuildingBlocks)
                        .ByRelation<UseCase.DependsOnBuildingBlock>()))
                    .ToHashSet();
                var processes = modelGraph.Execute(query => query
                    .Elements<Process>()
                    .RelatedToAny(useCases)
                    .ByRelation<Process.ContainsUseCase>());
                var deployableUnits = modelGraph.GetDeployableUnitsFor(module);
                var teams = modelGraph.GetDevelopmentTeamsFor(module);
                var organizationalUnits = modelGraph.GetBusinessOrganizationalUnitsFor(module);
                var codeStructures = modelGraph.Execute(query => query
                    .Elements<CodeStructure>()
                    .RelatedTo(module)
                    .ByReverseRelation<DomainModule.IsImplementedBy>());
                return new DomainModulePage(outputDirectory, module, parent, children, processes, directBuildingBlocks,
                    deployableUnits, teams, organizationalUnits, codeStructures);
            });
    }
}
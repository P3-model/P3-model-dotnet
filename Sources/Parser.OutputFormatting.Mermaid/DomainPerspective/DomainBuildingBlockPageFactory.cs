using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Technology;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainBuildingBlockPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<DomainBuildingBlock>())
        .Where(buildingBlock => buildingBlock is not UseCase)
        .Select(buildingBlock =>
        {
            var module = modelGraph
                .Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo(buildingBlock)
                    .ByRelation<DomainModule.ContainsBuildingBlock>())
                .SingleOrDefault();
            var usingElements = modelGraph
                .Execute(query => query
                    .Elements<DomainBuildingBlock>()
                    .RelatedTo(buildingBlock)
                    .ByRelation<DomainBuildingBlock.DependsOnBuildingBlock>())
                .Select(dependency => (
                    Dependency: dependency,
                    Module: modelGraph
                        .Execute(query => query
                            .Elements<DomainModule>()
                            .RelatedTo(dependency)
                            .ByRelation<DomainModule.ContainsBuildingBlock>())
                        .SingleOrDefault()))
                .ToHashSet();
            var usedElements = modelGraph
                .Execute(query => query
                    .Elements<DomainBuildingBlock>()
                    .RelatedTo(buildingBlock)
                    .ByReverseRelation<DomainBuildingBlock.DependsOnBuildingBlock>())
                .Select(dependency => (
                    Dependency: dependency,
                    Module: modelGraph
                        .Execute(query => query
                            .Elements<DomainModule>()
                            .RelatedTo(dependency)
                            .ByRelation<DomainModule.ContainsBuildingBlock>())
                        .SingleOrDefault()))
                .ToHashSet();
            var codeStructures = modelGraph.Execute(query => query
                .Elements<CodeStructure>()
                .RelatedTo(buildingBlock)
                .ByReverseRelation<DomainBuildingBlock.IsImplementedBy>());
            return new DomainBuildingBlockPage(outputDirectory, buildingBlock, module, usingElements, usedElements,
                codeStructures);
        });
}
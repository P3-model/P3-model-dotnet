using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainBuildingBlockPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query.AllElements<DomainBuildingBlock>())
        .Select(buildingBlock =>
        {
            var module = modelGraph
                .Execute(query => query
                    .Elements<DomainModule>().RelatedTo(buildingBlock)
                    .ByRelation<DomainModule.ContainsBuildingBlock>())
                .SingleOrDefault();
            var dependencies = modelGraph
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
            var processSteps = modelGraph
                .Execute(query => query
                    .Elements<ProcessStep>()
                    .RelatedTo(buildingBlock)
                    .ByRelation<ProcessStep.DependsOnBuildingBlock>());
            return new DomainBuildingBlockPage(outputDirectory, buildingBlock, module, dependencies, processSteps);
        });
}
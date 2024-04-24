using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddRepositoryAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<DddRepository>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(DddRepositoryAttribute);

    protected override DddRepository CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddRepository>(idPartUniqueForElementType),  name);
}
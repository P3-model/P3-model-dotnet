using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddFactoryAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<DddFactory>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(DddFactoryAttribute);

    protected override DddFactory CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddFactory>(idPartUniqueForElementType),  name);
}
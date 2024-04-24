using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class DomainBuildingBlockAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<DomainBuildingBlock>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(DomainBuildingBlockAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string idPartUniqueForElementType, string name) =>
        new(ElementId.Create<DomainBuildingBlock>(idPartUniqueForElementType), name);
}
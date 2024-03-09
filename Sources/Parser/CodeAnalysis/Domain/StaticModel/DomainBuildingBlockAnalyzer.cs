using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

[UsedImplicitly]
public class DomainBuildingBlockAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<DomainBuildingBlock>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(DomainBuildingBlockAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string id, string name) => new(id, name);
}
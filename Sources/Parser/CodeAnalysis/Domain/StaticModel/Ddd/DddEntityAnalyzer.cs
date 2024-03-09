using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddEntityAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<DddEntity>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(DddEntityAttribute);

    protected override DddEntity CreateBuildingBlock(string id, string name) => new(id, name);
}
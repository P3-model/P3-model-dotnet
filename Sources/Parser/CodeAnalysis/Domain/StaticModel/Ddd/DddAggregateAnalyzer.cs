using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddAggregateAnalyzer(DomainModuleFinder moduleFinder)
    : DomainBuildingBlockAnalyzerBase<DddAggregate>(moduleFinder)
{
    protected override Type AttributeType => typeof(DddAggregateAttribute);

    protected override DddAggregate CreateBuildingBlock(string id, string name) => new(id, name);
}
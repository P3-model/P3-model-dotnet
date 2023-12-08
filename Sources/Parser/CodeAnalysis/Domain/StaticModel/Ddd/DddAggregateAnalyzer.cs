using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddAggregateAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(DddAggregateAttribute);

    public DddAggregateAnalyzer(DomainModuleFinder moduleFinder) : base(moduleFinder) { }

    protected override DomainBuildingBlock CreateBuildingBlock(string id, string name) => new DddAggregate(id, name);
}
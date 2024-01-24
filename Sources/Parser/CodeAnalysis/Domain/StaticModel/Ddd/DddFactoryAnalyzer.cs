using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddFactoryAnalyzer(DomainModuleFinder moduleFinder)
    : DomainBuildingBlockAnalyzerBase<DddFactory>(moduleFinder)
{
    protected override Type AttributeType => typeof(DddFactoryAttribute);

    protected override DddFactory CreateBuildingBlock(string id, string name) => new(id, name);
}
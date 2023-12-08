using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

[UsedImplicitly]
public class DomainBuildingBlockAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(DomainBuildingBlockAttribute);

    public DomainBuildingBlockAnalyzer(DomainModuleFinder moduleFinder) : base(moduleFinder) { }

    protected override DomainBuildingBlock CreateBuildingBlock(string id, string name) => new(id, name);
}
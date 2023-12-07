using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddDomainServiceAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(DddDomainServiceAttribute);

    public DddDomainServiceAnalyzer(DomainModuleFinder moduleFinder) : base(moduleFinder) { }

    protected override DomainBuildingBlock CreateBuildingBlock(DomainModule? module, string name) => 
        new DddDomainService(module, name);
}
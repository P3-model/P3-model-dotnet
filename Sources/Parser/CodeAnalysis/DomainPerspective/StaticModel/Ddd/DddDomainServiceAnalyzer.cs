using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel.Ddd;

[UsedImplicitly]
public class DddDomainServiceAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(DddDomainServiceAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string name) => new DddDomainService(name);
}
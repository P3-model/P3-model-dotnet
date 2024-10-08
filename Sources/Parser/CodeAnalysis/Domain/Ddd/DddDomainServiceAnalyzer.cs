using JetBrains.Annotations;
using P3Model.Annotations.Domain.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.Ddd;

[UsedImplicitly]
public class DddDomainServiceAnalyzer(ModelBoundaryAnalyzer modelBoundaryAnalyzer, 
    DomainModuleAnalyzer domainModuleAnalyzer)
    : DomainBuildingBlockAnalyzerBase<DddDomainService>(modelBoundaryAnalyzer, domainModuleAnalyzer)
{
    protected override Type AttributeType => typeof(DddDomainServiceAttribute);

    protected override DddDomainService CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddDomainService>(idPartUniqueForElementType),  name);
}
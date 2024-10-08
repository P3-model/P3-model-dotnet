using JetBrains.Annotations;
using P3Model.Annotations.Domain.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.Ddd;

[UsedImplicitly]
public class DddEntityAnalyzer(ModelBoundaryAnalyzer modelBoundaryAnalyzer, 
    DomainModuleAnalyzer domainModuleAnalyzer)
    : DomainBuildingBlockAnalyzerBase<DddEntity>(modelBoundaryAnalyzer, domainModuleAnalyzer)
{
    protected override Type AttributeType => typeof(DddEntityAttribute);

    protected override DddEntity CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddEntity>(idPartUniqueForElementType),  name);
}
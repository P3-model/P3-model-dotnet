using JetBrains.Annotations;
using P3Model.Annotations.Domain.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.Ddd;

[UsedImplicitly]
public class DddAggregateAnalyzer(ModelBoundaryAnalyzer modelBoundaryAnalyzer, 
    DomainModuleAnalyzer domainModuleAnalyzer)
    : DomainBuildingBlockAnalyzerBase<DddAggregate>(modelBoundaryAnalyzer, domainModuleAnalyzer)
{
    protected override Type AttributeType => typeof(DddAggregateAttribute);

    protected override DddAggregate CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddAggregate>(idPartUniqueForElementType),  name);
}
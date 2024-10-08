using JetBrains.Annotations;
using P3Model.Annotations.Domain.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.Ddd;

[UsedImplicitly]
public class DddValueObjectAnalyzer(ModelBoundaryAnalyzer modelBoundaryAnalyzer, 
    DomainModuleAnalyzer domainModuleAnalyzer)
    : DomainBuildingBlockAnalyzerBase<DddValueObject>(modelBoundaryAnalyzer, domainModuleAnalyzer)
{
    protected override Type AttributeType => typeof(DddValueObjectAttribute);

    protected override DddValueObject CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddValueObject>(idPartUniqueForElementType),  name);
}
using JetBrains.Annotations;
using P3Model.Annotations.Domain;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class DomainBuildingBlockAnalyzer(ModelBoundaryAnalyzer modelBoundaryAnalyzer, 
    DomainModuleAnalyzer domainModuleAnalyzer)
    : DomainBuildingBlockAnalyzerBase<DomainBuildingBlock>(modelBoundaryAnalyzer, domainModuleAnalyzer)
{
    protected override Type AttributeType => typeof(DomainBuildingBlockAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string idPartUniqueForElementType, string name) =>
        new(ElementId.Create<DomainBuildingBlock>(idPartUniqueForElementType), name);
}
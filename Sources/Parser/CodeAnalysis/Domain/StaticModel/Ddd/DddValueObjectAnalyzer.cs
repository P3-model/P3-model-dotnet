using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddValueObjectAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<DddValueObject>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(DddValueObjectAttribute);

    protected override DddValueObject CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<DddValueObject>(idPartUniqueForElementType),  name);
}
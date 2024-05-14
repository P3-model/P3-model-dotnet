using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class ExternalSystemIntegrationAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<ExternalSystemIntegration>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(ExternalSystemIntegrationAttribute);

    protected override ExternalSystemIntegration CreateBuildingBlock(string idPartUniqueForElementType, string name) =>
        new(ElementId.Create<ExternalSystemIntegration>(idPartUniqueForElementType), name);

    protected override void AddElementsAndRelations(ExternalSystemIntegration buildingBlock,
        DomainModule? module, ISymbol symbol, AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        base.AddElementsAndRelations(buildingBlock, module, symbol, buildingBlockAttribute, modelBuilder);
        var name = buildingBlockAttribute.GetConstructorArgumentValue<string>(
            nameof(ExternalSystemIntegrationAttribute.ExternalSystemName));
        var externalSystem = new ExternalSystem(
            ElementId.Create<ExternalSystem>(name.Dehumanize()), 
            name.Humanize(LetterCasing.Title));
        modelBuilder.Add(externalSystem);
        modelBuilder.Add(new ExternalSystemIntegration.Integrates(buildingBlock, externalSystem));
    }
}
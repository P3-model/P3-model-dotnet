using System;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

[UsedImplicitly]
public class ExternalSystemIntegrationAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<ExternalSystemIntegration>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(ExternalSystemIntegrationAttribute);

    protected override ExternalSystemIntegration CreateBuildingBlock(string id, string name) => new(id, name);

    protected override void AddElementsAndRelations(ExternalSystemIntegration buildingBlock,
        DomainModule? module, ISymbol symbol, AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        base.AddElementsAndRelations(buildingBlock, module, symbol, buildingBlockAttribute, modelBuilder);
        var externalSystemName = buildingBlockAttribute.GetConstructorArgumentValue<string>(
            nameof(ExternalSystemIntegrationAttribute.ExternalSystemName));
        var externalSystem = new ExternalSystem(externalSystemName);
        modelBuilder.Add(externalSystem);
        modelBuilder.Add(new ExternalSystemIntegration.Integrates(buildingBlock, externalSystem));
    }
}
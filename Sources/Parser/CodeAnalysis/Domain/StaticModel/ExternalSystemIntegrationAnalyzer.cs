using System;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

[UsedImplicitly]
public class ExternalSystemIntegrationAnalyzer(DomainModuleFinder moduleFinder)
    : DomainBuildingBlockAnalyzerBase<ExternalSystemIntegration>(moduleFinder)
{
    protected override Type AttributeType => typeof(ExternalSystemIntegrationAttribute);

    protected override ExternalSystemIntegration CreateBuildingBlock(string id, string name) => new(id, name);

    protected override void AddElementsAndRelations(ExternalSystemIntegration externalSystemIntegration,
        DomainModule? module, ISymbol symbol, AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        base.AddElementsAndRelations(externalSystemIntegration, module, symbol, buildingBlockAttribute, modelBuilder);
        var externalSystemName = buildingBlockAttribute.GetConstructorArgumentValue<string>(
            nameof(ExternalSystemIntegrationAttribute.ExternalSystemName));
        var externalSystem = new ExternalSystem(externalSystemName);
        modelBuilder.Add(externalSystem);
        modelBuilder.Add(new ExternalSystemIntegration.Integrates(externalSystemIntegration, externalSystem));
    }
}
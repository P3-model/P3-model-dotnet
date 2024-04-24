using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class UseCaseAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<UseCase>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(UseCaseAttribute);

    protected override UseCase CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<UseCase>(idPartUniqueForElementType), name);

    protected override void AddElementsAndRelations(UseCase useCase, DomainModule? module, ISymbol symbol,
        AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        base.AddElementsAndRelations(useCase, module, symbol, buildingBlockAttribute, modelBuilder);
        modelBuilder.Add(elements => GetRelationsToProcesses(useCase, buildingBlockAttribute, elements));
    }

    private static IEnumerable<Relation> GetRelationsToProcesses(UseCase useCase, AttributeData useCaseAttribute,
        ElementsProvider elements)
    {
        if (!TryGetProcessName(useCaseAttribute, out var processName))
            yield break;
        foreach (var process in elements
                     .OfType<Process>()
                     .Where(p => p.Name.Equals(processName, StringComparison.InvariantCulture)))
            yield return new Process.ContainsUseCase(process, useCase);
    }

    private static bool TryGetProcessName(AttributeData useCaseAttribute,
        [NotNullWhen(true)] out string? processName)
    {
        if (useCaseAttribute.TryGetConstructorArgumentValue(nameof(UseCaseAttribute.Process), out processName))
            return true;
        if (useCaseAttribute.TryGetNamedArgumentValue(nameof(UseCaseAttribute.Process), out processName))
            return true;
        return false;
    }
}
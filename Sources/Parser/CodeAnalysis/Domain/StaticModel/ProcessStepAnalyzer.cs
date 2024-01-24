using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

[UsedImplicitly]
public class ProcessStepAnalyzer(DomainModuleFinder moduleFinder)
    : DomainBuildingBlockAnalyzerBase<ProcessStep>(moduleFinder)
{
    protected override Type AttributeType => typeof(ProcessStepAttribute);

    protected override ProcessStep CreateBuildingBlock(string id, string name) => new(id, name);

    protected override void AddElementsAndRelations(ProcessStep processStep, DomainModule? module, ISymbol symbol,
        AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        base.AddElementsAndRelations(processStep, module, symbol, buildingBlockAttribute, modelBuilder);
        modelBuilder.Add(elements => GetRelationsToNextSteps(processStep, buildingBlockAttribute, elements));
        modelBuilder.Add(elements => GetRelationsToProcesses(processStep, buildingBlockAttribute, elements));
    }

    private static IEnumerable<Relation> GetRelationsToProcesses(ProcessStep step, AttributeData stepAttribute,
        ElementsProvider elements)
    {
        if (!TryGetProcessName(stepAttribute, out var processName))
            yield break;
        foreach (var process in elements
                     .OfType<Process>()
                     .Where(p => p.Name.Equals(processName, StringComparison.InvariantCulture)))
            yield return new Process.ContainsProcessStep(process, step);
    }

    private static IEnumerable<Relation> GetRelationsToNextSteps(ProcessStep step, AttributeData stepAttribute,
        ElementsProvider elements)
    {
        if (!TryGetNextStepNames(stepAttribute, out var nextStepNames))
            yield break;
        foreach (var nextStepName in nextStepNames)
        {
            var nextSteps = elements
                .OfType<ProcessStep>()
                .Where(s => s.Name.Equals(nextStepName, StringComparison.InvariantCulture))
                .ToList();
            // TODO: warning logging if step not found
            // TODO: warning logging if more than one element (non unique names o process steps)
            if (nextSteps.Count == 1)
                yield return new ProcessStep.HasNextStep(step, nextSteps[0]);
        }
    }

    private static bool TryGetProcessName(AttributeData stepAttribute,
        [NotNullWhen(true)] out string? processName)
    {
        if (stepAttribute.TryGetConstructorArgumentValue(nameof(ProcessStepAttribute.Process), out processName))
            return true;
        if (stepAttribute.TryGetNamedArgumentValue(nameof(ProcessStepAttribute.Process), out processName))
            return true;
        return false;
    }

    private static bool TryGetNextStepNames(AttributeData stepAttribute,
        [NotNullWhen(true)] out IEnumerable<string>? nextStepNames)
    {
        if (stepAttribute.TryGetConstructorArgumentValues(nameof(ProcessStepAttribute.NextSteps), out nextStepNames))
            return true;
        if (stepAttribute.TryGetNamedArgumentValues(nameof(ProcessStepAttribute.NextSteps), out nextStepNames))
            return true;
        return false;
    }
}
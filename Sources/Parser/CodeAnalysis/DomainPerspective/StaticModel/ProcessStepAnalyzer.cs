using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

[UsedImplicitly]
public class ProcessStepAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(ProcessStepAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string name) =>
        new ProcessStep(HierarchyId.FromValue(name));

    protected override IEnumerable<Relation> GetRelations(ISymbol symbol, DomainBuildingBlock buildingBlock,
        AttributeData buildingBlockAttribute, ElementsProvider elements) => base
        .GetRelations(symbol, buildingBlock, buildingBlockAttribute, elements)
        .Union(GetRelationsToNextSteps((ProcessStep)buildingBlock, buildingBlockAttribute, elements))
        .Union(GetRelationsToProcesses((ProcessStep)buildingBlock, buildingBlockAttribute, elements));

    private static IEnumerable<Relation> GetRelationsToProcesses(ProcessStep step, AttributeData stepAttribute, 
        ElementsProvider elements)
    {
        if (!TryGetProcessName(stepAttribute, out var processFullName)) 
            yield break;
        var processes = elements
            .OfType<Process>()
            .Where(p => p.Id.Full.Equals(processFullName, StringComparison.InvariantCulture))
            .ToList();
        // TODO: warning logging if process not found
        // TODO: warning logging if more than one element (non unique names o processes)
        if (processes.Count == 1)
            yield return new Process.ContainsProcessStep(processes[0], step);
    }

    private static IEnumerable<Relation> GetRelationsToNextSteps(ProcessStep step, AttributeData stepAttribute, 
        ElementsProvider elements)
    {
        if (!TryGetNextStepNames(stepAttribute, out var nextStepNames))
            yield break;
        foreach (var nextStepFullName in nextStepNames)
        {
            var nextSteps = elements
                .OfType<ProcessStep>()
                .Where(s => s.Id.Full.Equals(nextStepFullName, StringComparison.InvariantCulture))
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
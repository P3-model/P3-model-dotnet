using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

[UsedImplicitly]
public class ProcessStepAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(ProcessStepAttribute);

    public ProcessStepAnalyzer(DomainModuleFinder moduleFinder) : base(moduleFinder) { }

    protected override DomainBuildingBlock CreateBuildingBlock(DomainModule? module, string name) =>
        new ProcessStep(module, name);

    protected override IEnumerable<Relation> GetRelations(DomainBuildingBlock buildingBlock,
        AttributeData buildingBlockAttribute, ElementsProvider elements) => base
        .GetRelations(buildingBlock, buildingBlockAttribute, elements)
        .Union(GetRelationsToNextSteps((ProcessStep)buildingBlock, buildingBlockAttribute, elements))
        .Union(GetRelationsToProcesses((ProcessStep)buildingBlock, buildingBlockAttribute, elements));

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
        foreach (var nextStepFullName in nextStepNames)
        {
            var lastSeparatorIndex = nextStepFullName.LastIndexOf('.');
            var nextStepModuleFullName = lastSeparatorIndex == -1 ? null : nextStepFullName[..lastSeparatorIndex];
            var nextStepName = lastSeparatorIndex == -1 ? nextStepFullName : nextStepFullName[lastSeparatorIndex..];
            var nextSteps = elements
                .OfType<ProcessStep>()
                .Where(s => ((s.Module == null && string.IsNullOrEmpty(nextStepModuleFullName)) ||
                             (s.Module != null && s.Module.FullName.Equals(nextStepModuleFullName,
                                 StringComparison.InvariantCulture))) &&
                            s.Name.Equals(nextStepName, StringComparison.InvariantCulture))
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
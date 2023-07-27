using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.DynamicModel;

[UsedImplicitly]
public class ProcessStepAnalyzer : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    private static void Analyze(ISymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ProcessStepAttribute), out var stepAttribute))
            return;
        var name = stepAttribute.GetConstructorArgumentValue<string?>(nameof(ProcessStepAttribute.Name)) ?? symbol.Name;
        var stepId = TryGetProcessName(stepAttribute, out var processFullName)
            ? HierarchyId.FromParts(processFullName, name)
            : HierarchyId.FromValue(name);
        var step = new ProcessStep(stepId);
        modelBuilder.Add(step, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, stepAttribute, step, elements));
    }

    private static IEnumerable<Relation> GetRelations(ISymbol symbol, AttributeData stepAttribute,
        ProcessStep step, ElementsProvider elements)
    {
        if (TryGetProcessName(stepAttribute, out var processFullName))
        {
            var processes = elements
                .OfType<Process>()
                .Where(p => p.Id.Full.Equals(processFullName, StringComparison.InvariantCulture))
                .ToList();
            // TODO: warning logging if process not found
            // TODO: warning logging if more than one element (non unique names o processes)
            if (processes.Count == 1)
                yield return new Process.ContainsProcessStep(processes[0], step);
        }
        if (TryGetNextStepNames(stepAttribute, out var nextStepNames))
        {
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

        var module = elements
            .For(symbol.ContainingNamespace)
            .OfType<DomainModule>()
            .SingleOrDefault();
        if (module != null)
            yield return new ProcessStep.BelongsToDomainModule(step, module);
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
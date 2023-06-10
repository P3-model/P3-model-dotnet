using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.DynamicModel;

[UsedImplicitly]
public class ProcessStepAnalyzer : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    private static void Analyze(ISymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ProcessStepAttribute), out var processStepAttribute))
            return;
        var name = processStepAttribute.GetConstructorArgumentValue<string?>(nameof(ProcessStepAttribute.Name))
                   ?? symbol.Name;
        var processStep = new ProcessStep(name);
        modelBuilder.Add(processStep, symbol);
        modelBuilder.Add(elements => GetRelations(processStepAttribute, processStep, elements));
    }

    private static IEnumerable<Relation> GetRelations(AttributeData processStepAttribute, ProcessStep step,
        ElementsProvider elements)
    {
        if (TryGetProcessName(processStepAttribute, out var processName))
        {
            var process = elements.OfType<Process>().SingleOrDefault(p => p.Name == processName);
            // TODO: warning logging if process not found
            if (process != null)
                yield return new Process.ContainsProcessStep(process, step);
        }
        if (TryGetNextStepNames(processStepAttribute, out var nextStepNames))
        {
            foreach (var nextStepName in nextStepNames)
            {
                var nextStep = elements.OfType<ProcessStep>().SingleOrDefault(s => s.Name == nextStepName);
                // TODO: warning logging if step not found
                if (nextStep != null)
                    yield return new ProcessStep.HasNextStep(step, nextStep);
            }
        }
    }

    private static bool TryGetProcessName(AttributeData processStepAttribute, 
        [NotNullWhen(true)] out string? processName)
    {
        if (processStepAttribute.TryGetConstructorArgumentValue(nameof(ProcessStepAttribute.Process), out processName))
            return true;
        if (processStepAttribute.TryGetNamedArgumentValue(nameof(ProcessStepAttribute.Process), out processName))
            return true;
        return false;
    }
    
    private static bool TryGetNextStepNames(AttributeData processStepAttribute, 
        [NotNullWhen(true)] out IEnumerable<string>? nextStepNames)
    {
        if (processStepAttribute.TryGetConstructorArgumentValues(nameof(ProcessStepAttribute.NextSteps), out nextStepNames))
            return true;
        if (processStepAttribute.TryGetNamedArgumentValues(nameof(ProcessStepAttribute.NextSteps), out nextStepNames))
            return true;
        return false;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.DynamicModel;

[UsedImplicitly]
public class ProcessAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ProcessAttribute), out var processAttribute))
            return;
        var fullName = processAttribute.GetConstructorArgumentValues<string>(nameof(ProcessAttribute.FullName));
        var process = new Process(HierarchyId.FromParts(fullName));
        if (!processAttribute.TryGetNamedArgumentValue<bool>(nameof(ProcessAttribute.ApplyOnNamespace),
                out var applyOnNamespace))
            applyOnNamespace = false;
        modelBuilder.Add(process, applyOnNamespace ? symbol.ContainingNamespace : symbol);
        modelBuilder.Add(elements => GetRelations(process, processAttribute, elements));
    }

    private static IEnumerable<Relation> GetRelations(Process process, AttributeData processAttribute,
        ElementsProvider elements)
    {
        var parent = process.Id.Level > 0
            ? elements
                .OfType<Process>()
                .SingleOrDefault(p => p.Id.FullName
                    .Equals(process.Id.ParentFullName, StringComparison.InvariantCulture))
            : null;
        // TODO: warning logging if parent not found
        if (parent != null)
            yield return new Process.ContainsSubProcess(parent, process);
        if (!processAttribute.TryGetNamedArgumentValues<string>(nameof(ProcessAttribute.NextSubProcesses),
                out var nextSubProcesses))
            yield break;
        foreach (var nextSubProcessName in nextSubProcesses)
        {
            var nextSubProcess = elements
                .OfType<Process>()
                .SingleOrDefault(p => p.Id.FullName
                    .Equals(nextSubProcessName, StringComparison.InvariantCulture));
            // TODO: warning logging if nextProcess not found
            if (nextSubProcess != null)
                yield return new Process.HasNextSubProcess(process, nextSubProcess);
        }
    }
}
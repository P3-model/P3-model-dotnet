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
        var name = processAttribute.GetConstructorArgumentValue<string?>(nameof(ProcessAttribute.Name)) ?? symbol.Name;
        var process = new Process(name);
        var applyOnNamespace = processAttribute.GetConstructorArgumentValue<bool>(
            nameof(ProcessAttribute.ApplyOnNamespace));
        modelBuilder.Add(process, applyOnNamespace ? symbol.ContainingNamespace : symbol);
        modelBuilder.Add(elements => GetRelations(processAttribute, process, elements));
    }

    private static IEnumerable<Relation> GetRelations(AttributeData processAttribute, Process child,
        ElementsProvider elements)
    {
        if (!processAttribute.TryGetNamedArgumentValue<string?>(nameof(ProcessAttribute.Parent), out var parentName))
            yield break;
        var parent = elements.OfType<Process>().SingleOrDefault(p => p.Name == parentName);
        // TODO: warning logging if parent not found
        if (parent != null)
            yield return new Process.ContainsSubProcess(parent, child);
    }
}
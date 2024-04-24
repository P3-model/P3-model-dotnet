using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class CodeStructureAnalyzer : SymbolAnalyzer<IAssemblySymbol>,
    SymbolAnalyzer<INamespaceSymbol>,
    SymbolAnalyzer<INamedTypeSymbol>
{
    // TODO: getting source code path
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        var cSharpProject = new CSharpProject(
            ElementId.Create<CSharpProject>(symbol.Name),
            symbol.Name,
            string.Empty);
        modelBuilder.Add(cSharpProject, symbol);
        modelBuilder.Add(elements => symbol
            .GetReferencedAssembliesFromSameRepository()
            .SelectMany(referencedSymbol => elements
                .For(referencedSymbol)
                .OfType<CSharpProject>())
            .Select(referencedProject => new CSharpProject.ReferencesProject(cSharpProject, referencedProject)));
    }

    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        var cSharpNamespace = new CSharpNamespace(
            ElementId.Create<CSharpNamespace>(symbol.ToDisplayString()),
            HierarchyPath.FromValue(symbol.ToDisplayString()),
            string.Empty);
        modelBuilder.Add(cSharpNamespace, symbol);
        if (symbol.ContainingNamespace.IsGlobalNamespace)
            modelBuilder.Add(elements => elements
                .For(symbol.ContainingAssembly)
                .OfType<CSharpProject>()
                .Select(cSharpProject => new CSharpProject.ContainsNamespace(cSharpProject, cSharpNamespace)));
        else
            modelBuilder.Add(elements => elements
                .For(symbol.ContainingNamespace)
                .OfType<CSharpNamespace>()
                .Select(parent => new CSharpNamespace.ContainsNamespace(parent, cSharpNamespace)));
    }

    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        var cSharpType = new CSharpType(
            ElementId.Create<CSharpType>(symbol.ToDisplayString()),
            symbol.GetFullName(),
            string.Empty);
        modelBuilder.Add(cSharpType, symbol);
        modelBuilder.Add(elements => elements
            .For(symbol.ContainingNamespace)
            .OfType<CSharpNamespace>()
            .Select(cSharpNamespace => new CSharpNamespace.ContainsType(cSharpNamespace, cSharpType)));
    }
}
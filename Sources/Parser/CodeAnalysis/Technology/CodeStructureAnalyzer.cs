using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class CodeStructureAnalyzer : SymbolAnalyzer<IAssemblySymbol>,
    SymbolAnalyzer<INamespaceSymbol>,
    SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (symbol.TryGetAttribute(typeof(ExcludeFromDocsAttribute), out _))
            return;
        var cSharpProject = new CSharpProject(symbol.Name, 
            Path.GetDirectoryName(symbol.GetSourceCodePaths().First())!);
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
        // TODO: getting all folders for namespace
        var cSharpNamespace = new CSharpNamespace(
            HierarchyId.FromValue(symbol.ToDisplayString()),
            Path.GetDirectoryName(symbol.GetSourceCodePaths().First())!);
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
            HierarchyId.FromValue(symbol.ToDisplayString()),
            symbol.GetFullName(),
            symbol.GetSourceCodePaths().First());
        modelBuilder.Add(cSharpType, symbol);
        modelBuilder.Add(elements => elements
            .For(symbol.ContainingNamespace)
            .OfType<CSharpNamespace>()
            .Select(cSharpNamespace => new CSharpNamespace.ContainsType(cSharpNamespace, cSharpType)));
    }
}
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

public class ModelBoundaryAnalyzer : SymbolAnalyzer<IAssemblySymbol>
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        foreach (var modelBoundaryAttribute in symbol.GetAttributes(typeof(ModelBoundaryAttribute)))
        {
            var name = modelBoundaryAttribute.ConstructorArguments[0].Value?.ToString()
                       ?? symbol.Name;
            modelBuilder.Add(new ModelBoundary(name), symbol);
        }
    }
}
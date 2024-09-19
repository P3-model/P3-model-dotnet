using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class DomainBuildingBlocksDependenciesAnalyzer : SymbolAnalyzer<IFieldSymbol>,
    SymbolAnalyzer<IMethodSymbol>,
    SymbolAnalyzer<ILocalSymbol>,
    SymbolAnalyzer<IParameterSymbol>,
    SymbolAnalyzer<IPropertySymbol>,
    OperationAnalyzer<IInvocationOperation>
{
    public void Analyze(IFieldSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = UnwrapFromCollectionAndNullable(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        // Only return type is needed because parameters are analyzed separately through IParameterSymbol.
        var destinationSymbol = UnwrapFromCollectionAndNullable(symbol.ReturnType);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(ILocalSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = UnwrapFromCollectionAndNullable(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IParameterSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = UnwrapFromCollectionAndNullable(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IPropertySymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = UnwrapFromCollectionAndNullable(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IInvocationOperation operation, ModelBuilder modelBuilder)
    {
        if (!TryGetContainingSymbols(operation, out var containingType, out var containingMethod))
            return;
        var invokedMethod = operation.TargetMethod;
        var invokedMethodContainingType = invokedMethod.ContainingType;
        modelBuilder.Add(elements => CreateRelations(containingType, invokedMethod, elements));
        modelBuilder.Add(elements => CreateRelations(containingType, invokedMethodContainingType, elements));
        modelBuilder.Add(elements => CreateRelations(containingMethod, invokedMethod, elements));
        modelBuilder.Add(elements => CreateRelations(containingMethod, invokedMethodContainingType, elements));
        foreach (var parameter in invokedMethod.Parameters.Select(p => UnwrapFromCollectionAndNullable(p.Type)))
        {
            modelBuilder.Add(elements => CreateRelations(containingType, parameter, elements));
            modelBuilder.Add(elements => CreateRelations(containingMethod, parameter, elements));
        }
    }

    private static bool TryGetContainingSymbols(IOperation operation,
        [NotNullWhen(true)] out INamedTypeSymbol? typeSymbol,
        [NotNullWhen(true)] out IMethodSymbol? methodSymbol)
    {
        methodSymbol = operation.SemanticModel?.GetEnclosingSymbol(operation.Syntax.SpanStart) as IMethodSymbol;
        if (methodSymbol is null)
        {
            // This can happen when the operation is a field initializer.
            // Fields are analyzed separately through IFieldSymbol.
            typeSymbol = null;
            return false;
        }
        typeSymbol = methodSymbol.ContainingType;
        return true;
    }

    private static ITypeSymbol UnwrapFromCollectionAndNullable(ITypeSymbol symbol)
    {
        while (true)
        {
            switch (symbol)
            {
                case INamedTypeSymbol { IsGenericType: true } genericTypeSymbol
                    when IsSystemCollection(genericTypeSymbol) || IsSystemTask(genericTypeSymbol):
                    symbol = genericTypeSymbol.TypeArguments[0];
                    break;
                case IArrayTypeSymbol arrayTypeSymbol:
                    symbol = arrayTypeSymbol.ElementType;
                    break;
                default:
                    return symbol.NullableAnnotation == NullableAnnotation.Annotated
                        ? symbol.OriginalDefinition
                        : symbol;
            }
        }
    }

    private static bool IsSystemCollection(INamedTypeSymbol genericTypeSymbol) =>
        genericTypeSymbol.ContainingNamespace.ToDisplayString().StartsWith("System.Collections") &&
        genericTypeSymbol.TypeParameters.Length == 1;

    private static bool IsSystemTask(INamedTypeSymbol genericTypeSymbol) =>
        genericTypeSymbol.ContainingNamespace.ToDisplayString().StartsWith("System.Threading.Tasks") &&
        genericTypeSymbol is { Name: nameof(Task), TypeParameters.Length: 1 };

    private static IEnumerable<Relation> CreateRelations(ISymbol sourceSymbol, ISymbol destinationSymbol,
        ElementsProvider elements)
    {
        var sources = elements.For(sourceSymbol).OfType<DomainBuildingBlock>();
        var destinations = elements.For(destinationSymbol).OfType<DomainBuildingBlock>().ToList();
        foreach (var source in sources)
        foreach (var destination in destinations.Where(destination => !source.Equals(destination)))
            yield return CreateRelation(source, destination);
    }

    private static DomainBuildingBlock.DependsOnBuildingBlock CreateRelation(DomainBuildingBlock source,
        DomainBuildingBlock destination) => source switch
    {
        UseCase useCase => new UseCase.DependsOnBuildingBlock(useCase, destination),
        _ => new DomainBuildingBlock.DependsOnBuildingBlock(source, destination)
    };
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

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
        var destinationSymbol = GetDestinationSymbol(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = GetDestinationSymbol(symbol.ReturnType); // Parameters are analyzed separately through IParameterSymbol
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }


    public void Analyze(ILocalSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = GetDestinationSymbol(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IParameterSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = GetDestinationSymbol(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IPropertySymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = GetDestinationSymbol(symbol.Type);
        modelBuilder.Add(elements => CreateRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IInvocationOperation operation, ModelBuilder modelBuilder)
    {
        var callingType = GetCallingType(operation);
        var methodContainingType = GetMethodContainingType(operation);
        if (callingType is null)
            return;
        if (CompilationIndependentSymbolEqualityComparer.Default.Equals(callingType, methodContainingType))
            return;
        modelBuilder.Add(elements => CreateRelations(callingType, methodContainingType, elements));
        foreach (var parameterSymbol in operation.TargetMethod.Parameters)
        {
            var destinationSymbol = GetDestinationSymbol(parameterSymbol.Type);
            modelBuilder.Add(elements => CreateRelations(callingType, destinationSymbol, elements));
        }
    }

    private static ITypeSymbol? GetCallingType(IInvocationOperation operation) => operation.Instance switch
    {
        IFieldReferenceOperation fieldReferenceOperation => fieldReferenceOperation.Field.ContainingType,
        IPropertyReferenceOperation propertyReferenceOperation => propertyReferenceOperation.Property.ContainingType,
        IInstanceReferenceOperation instanceReferenceOperation => instanceReferenceOperation.Type,
        ILocalReferenceOperation localReferenceOperation => localReferenceOperation.Local.ContainingType,
        // TODO: support for static method invocations
        _ => null
    };

    private static ITypeSymbol GetMethodContainingType(IInvocationOperation operation) =>
        operation.TargetMethod.ContainingType;

    private static ITypeSymbol GetDestinationSymbol(ITypeSymbol symbol)
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
        ProcessStep processStep => new ProcessStep.DependsOnBuildingBlock(processStep, destination),
        _ => new DomainBuildingBlock.DependsOnBuildingBlock(source, destination)
    };
}
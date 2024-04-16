using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis;

public interface OperationAnalyzer;

public interface OperationAnalyzer<in TOperation> : OperationAnalyzer
    where TOperation : IOperation
{
    void Analyze(TOperation operation, ModelBuilder modelBuilder);
}
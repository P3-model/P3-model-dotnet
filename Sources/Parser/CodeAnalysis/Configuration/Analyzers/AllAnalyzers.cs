using System.Collections.Generic;

namespace P3Model.Parser.CodeAnalysis.Configuration.Analyzers;

public record AllAnalyzers(IReadOnlyCollection<FileAnalyzer> FileAnalyzers,
    IReadOnlyCollection<SymbolAnalyzer> SymbolAnalyzers,
    IReadOnlyCollection<OperationAnalyzer> OperationAnalyzers);
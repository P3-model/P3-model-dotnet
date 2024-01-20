using System.Collections.Generic;
using P3Model.Parser.CodeAnalysis;

namespace P3Model.Parser.Configuration.Analyzers;

public record AllAnalyzers(IReadOnlyCollection<FileAnalyzer> FileAnalyzers,
    IReadOnlyCollection<SymbolAnalyzer> SymbolAnalyzers,
    IReadOnlyCollection<OperationAnalyzer> OperationAnalyzers);
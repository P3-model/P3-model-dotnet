using System.Collections.Generic;

namespace P3Model.Parser.CodeAnalysis;

public record RepositoryToAnalyze(string Path, 
    IReadOnlyCollection<string> SlnPaths, 
    IReadOnlyCollection<string> ExcludedProjects);
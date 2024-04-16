namespace P3Model.Parser.CodeAnalysis;

public interface FileAnalyzer
{
    Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder);
}
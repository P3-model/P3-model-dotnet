using System.IO;
using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

public interface FileAnalyzer
{
    Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder);
}
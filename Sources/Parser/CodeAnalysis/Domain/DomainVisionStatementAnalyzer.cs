using JetBrains.Annotations;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class DomainVisionStatementAnalyzer : FileAnalyzer
{
    public Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder)
    {
        // TODO: new concept for DomainVisionStatement
        // if (fileInfo.Name.Equals("DomainVisionStatement.md", StringComparison.InvariantCultureIgnoreCase))
        //     modelBuilder.Add(elements => GetTraits(fileInfo, elements));
        return Task.CompletedTask;
    }

    // private static IEnumerable<Trait> GetTraits(FileInfo fileInfo, ElementsProvider elements)
    // {
    //     yield return new DomainVisionStatement(fileInfo);
    // }
}
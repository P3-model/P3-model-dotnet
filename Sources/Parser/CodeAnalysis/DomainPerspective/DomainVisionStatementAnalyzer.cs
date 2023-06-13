using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

[UsedImplicitly]
public class DomainVisionStatementAnalyzer : FileAnalyzer
{
    public Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder)
    {
        if (fileInfo.Name.Equals("DomainVisionStatement.md", StringComparison.InvariantCultureIgnoreCase))
            modelBuilder.Add(elements => GetTraits(fileInfo, elements));
        return Task.CompletedTask;
    }

    private static IEnumerable<Trait> GetTraits(FileInfo fileInfo, ElementsProvider elements)
    {
        yield return new DomainVisionStatement(elements.OfType<Product>().Single(), fileInfo);
    }
}
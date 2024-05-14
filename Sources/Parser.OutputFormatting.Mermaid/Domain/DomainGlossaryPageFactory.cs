using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.Domain;

[UsedImplicitly]
public class DomainGlossaryPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DomainGlossaryPage(outputDirectory,
            modelGraph.Execute(query => query
                .Hierarchy<DomainModule, DomainModule.ContainsDomainModule>()),
            modelGraph.Execute(query => query
                    .Relations<DomainModule.ContainsBuildingBlock>())
                .GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => (IReadOnlyCollection<DomainBuildingBlock>)g
                    .Select(r => r.Destination)
                    .ToList()
                    .AsReadOnly()));
    }
}
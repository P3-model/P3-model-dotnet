using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainModulesPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DomainModulesPage(outputDirectory,
            modelGraph.Execute(query => query
                .Hierarchy<DomainModule, DomainModule.ContainsDomainModule>()));
    }
}
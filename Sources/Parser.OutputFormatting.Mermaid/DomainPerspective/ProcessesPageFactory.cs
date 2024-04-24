using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class ProcessesPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new ProcessesPage(outputDirectory,
            modelGraph.Execute(query => query
                .AllElements<Process>()));
    }
}
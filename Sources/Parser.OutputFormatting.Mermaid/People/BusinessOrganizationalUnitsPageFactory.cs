using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.People;

[UsedImplicitly]
public class BusinessOrganizationalUnitsPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new BusinessOrganizationalUnitsPage(outputDirectory,
            modelGraph.System,
            modelGraph.Execute(query => query
                .AllElements<BusinessOrganizationalUnit>()));
    }
}
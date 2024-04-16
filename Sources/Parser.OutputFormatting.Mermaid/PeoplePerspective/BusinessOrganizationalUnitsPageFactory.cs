using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.People;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

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
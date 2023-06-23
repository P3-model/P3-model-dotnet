using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class BusinessOrganizationalUnitsPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new BusinessOrganizationalUnitsPage(outputDirectory,
            modelGraph.Execute(query => query
                .SingleElement<Product>())!,
            modelGraph.Execute(query => query
                .AllElements<BusinessOrganizationalUnit>()));
    }
}
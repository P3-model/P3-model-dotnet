using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class BusinessOrganizationalUnitsPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new BusinessOrganizationalUnitsPage(outputDirectory,
            model.Cache.Product,
            model.Elements.OfType<BusinessOrganizationalUnit>());
    }
}
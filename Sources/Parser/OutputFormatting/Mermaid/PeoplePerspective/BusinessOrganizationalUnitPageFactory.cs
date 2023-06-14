using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class BusinessOrganizationalUnitPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        return model.Elements
            .OfType<BusinessOrganizationalUnit>()
            .Select(unit =>
            {
                var domainModules = model.Relations
                    .OfType<BusinessOrganizationalUnit.OwnsDomainModule>()
                    .Where(r => r.Unit.Equals(unit))
                    .Select(r => r.DomainModule)
                    .Distinct();
                return new BusinessOrganizationalUnitPage(outputDirectory, unit, domainModules);
            });
    }
}
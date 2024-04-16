using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class BusinessOrganizationalUnitPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<BusinessOrganizationalUnit>())
        .Select(unit =>
        {
            var domainModules = modelGraph.Execute(query => query
                .Elements<DomainModule>()
                .RelatedTo(unit)
                .ByReverseRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
            return new BusinessOrganizationalUnitPage(outputDirectory, unit, domainModules);
        });
}
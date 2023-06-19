using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelQuerying.Queries;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

[UsedImplicitly]
public class BusinessOrganizationalUnitPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(Query
            .Elements<BusinessOrganizationalUnit>()
            .All())
        .Select(unit =>
        {
            var domainModules = modelGraph.Execute(Query
                .Elements<DomainModule>()
                .RelatedTo(unit)
                .ByReverseRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
            return new BusinessOrganizationalUnitPage(outputDirectory, unit, domainModules);
        });
}
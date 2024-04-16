using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainModuleOwnersPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new DomainModuleOwnersPage(outputDirectory, GetModulesOwners(modelGraph));
    }

    private static IEnumerable<DomainModuleOwners> GetModulesOwners(ModelGraph modelGraph)
    {
        var modules = modelGraph.Execute(query => query
            .AllElements<DomainModule>(module => module.Level == 0));
        foreach (var module in modules)
        {
            var teams = modelGraph.Execute(query => query
                .Elements<DevelopmentTeam>()
                .RelatedToAny(subQuery => subQuery
                    .DescendantsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                .ByRelation<DevelopmentTeam.OwnsDomainModule>());
            var units = modelGraph.Execute(query => query
                .Elements<BusinessOrganizationalUnit>()
                .RelatedToAny(subQuery => subQuery
                    .DescendantsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
                .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
            yield return new DomainModuleOwners(module, teams, units);
        }
    }
}
using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

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
                .RelatedTo(module)
                .ByRelation<DevelopmentTeam.OwnsDomainModule>());
            var units = modelGraph.Execute(query => query
                .Elements<BusinessOrganizationalUnit>()
                .RelatedTo(module)
                .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
            yield return new DomainModuleOwners(module, teams, units);
        }
    }
}
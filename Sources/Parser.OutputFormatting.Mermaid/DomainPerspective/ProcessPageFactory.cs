using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class ProcessPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        var modulesHierarchy = modelGraph.HierarchyFor<DomainModule, DomainModule.ContainsDomainModule>();
        return modelGraph
            .Execute(query => query
                .AllElements<Process>())
            .Select(process =>
            {
                var useCases = modelGraph.Execute(query => query
                    .Elements<UseCase>()
                    .RelatedTo(process)
                    .ByReverseRelation<Process.ContainsUseCase>());
                var modules = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    // TODO: passing more specific objects as an argument to RelatedTo / RelatedToAny
                    .RelatedToAny(useCases.Cast<DomainBuildingBlock>().ToHashSet())
                    .ByRelation<DomainModule.ContainsBuildingBlock>());
                var topLevelModules = modules
                    .Select(module => modulesHierarchy.GetRootFor(module))
                    .ToHashSet();
                var deployableUnits = modelGraph.GetDeployableUnitsFor(modules);
                var actors = modelGraph.Execute(query => query
                    .Elements<Actor>()
                    .RelatedToAny(useCases)
                    .ByRelation<Actor.UsesUseCase>());
                var developmentTeams = modelGraph.GetDevelopmentTeamsFor(modules);
                var organizationalUnits = modelGraph.GetBusinessOrganizationalUnitsFor(modules);
                return new ProcessPage(outputDirectory, process, useCases, topLevelModules, deployableUnits!, actors, 
                    developmentTeams, organizationalUnits);
            });
    }
}
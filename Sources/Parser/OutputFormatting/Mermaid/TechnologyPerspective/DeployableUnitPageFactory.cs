using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

[UsedImplicitly]
public class DeployableUnitPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph) => modelGraph
        .Execute(query => query
            .AllElements<DeployableUnit>())
        .Select(unit =>
        {
            var tier = modelGraph.Execute(query => query
                    .Elements<Tier>()
                    .RelatedTo(unit)
                    .ByRelation<Tier.ContainsDeployableUnit>())
                .SingleOrDefault();
            var startupProject = modelGraph
                .Execute(query => query
                    .Elements<CSharpProject>()
                    .RelatedTo(unit)
                    .ByReverseRelation<DeployableUnit.ContainsCSharpProject>())
                .SingleOrDefault();
            var referencedProjects = startupProject != null
                ? modelGraph
                    .Execute(query => query
                        .Elements<CSharpProject>()
                        .RelatedTo(startupProject)
                        .ByReverseRelation<CSharpProject.ReferencesProject>())
                    .Select(project => (project, modelGraph
                        .Execute(query2 => query2
                            .Elements<Layer>()
                            .RelatedTo((CodeStructure)project)
                            .ByReverseRelation<CodeStructure.BelongsToLayer>())))
                    .ToHashSet()
                : new HashSet<(CSharpProject, IReadOnlySet<Layer>)>();
            var domainModules = modelGraph.Execute(query => query
                    .Elements<DomainModule>()
                    .RelatedTo(unit)
                    .ByRelation<DomainModule.IsDeployedInDeployableUnit>())
                .Where(m => m.Level == 0)
                .ToHashSet();
            var teams = modelGraph.Execute(query => query
                .Elements<DevelopmentTeam>()
                .RelatedToAny(domainModules)
                .ByRelation<DevelopmentTeam.OwnsDomainModule>());
            return new DeployableUnitPage(outputDirectory, unit, tier, startupProject, referencedProjects,
                domainModules, teams);
        });
}
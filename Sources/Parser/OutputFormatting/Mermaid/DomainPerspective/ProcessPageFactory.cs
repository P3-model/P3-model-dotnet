using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class ProcessPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        return model.Elements
            .OfType<Process>()
            .Select(process =>
            {
                var parent = model.Relations
                    .OfType<Process.ContainsSubProcess>()
                    .SingleOrDefault(r => r.Child == process)
                    ?.Parent;
                var children = new HashSet<Process>(model.Relations
                    .OfType<Process.ContainsSubProcess>()
                    .Where(r => r.Parent == process)
                    .Select(r => r.Child));
                var processHasNextSubProcessRelations = model.Relations
                    .OfType<Process.HasNextSubProcess>()
                    .Where(r => children.Contains(r.Current) && children.Contains(r.Next));
                var allSteps = new HashSet<ProcessStep>(model.Relations
                    .OfType<Process.ContainsProcessStep>()
                    .Where(r =>
                        r.Process.Equals(process) ||
                        r.Process.Hierarchy.IsDescendantOf(process.Hierarchy))
                    .Select(r => r.Step));
                var directSteps = model.Relations
                    .OfType<Process.ContainsProcessStep>()
                    .Where(r => r.Process.Equals(process))
                    .Select(r => r.Step);
                var domainModules = model.Relations
                    .OfType<ProcessStep.BelongsToDomainModule>()
                    .Where(r => allSteps.Contains(r.Step))
                    .Select(r => r.DomainModule)
                    .Select(m => model.Elements
                        .OfType<DomainModule>()
                        .Single(m2 => m2.Hierarchy.FullName == m.Hierarchy.RootFullName))
                    .Distinct()
                    .ToList();
                var deployableUnits = model.Relations
                    .OfType<DomainModule.IsDeployedInDeployableUnit>()
                    .Where(r => domainModules.Contains(r.DomainModule))
                    .Select(r => r.DeployableUnit)
                    .Distinct();
                var actors = model.Relations
                    .OfType<Actor.UsesProcessStep>()
                    .Where(r => allSteps.Contains(r.ProcessStep))
                    .Select(r => r.Actor)
                    .Distinct();
                var developmentTeams = domainModules
                    .SelectMany(m => model.Relations
                        .OfType<DevelopmentTeam.OwnsDomainModule>()
                        .Where(r => r.DomainModule == m)
                        .Select(r => r.Team))
                    .Distinct();
                var organizationalUnits = domainModules
                    .SelectMany(m => model.Relations
                        .OfType<BusinessOrganizationalUnit.OwnsDomainModule>()
                        .Where(r => r.DomainModule == m)
                        .Select(r => r.Unit))
                    .Distinct();
                return new ProcessPage(outputDirectory, process, parent, children, processHasNextSubProcessRelations,
                    directSteps, domainModules, deployableUnits, actors, developmentTeams, organizationalUnits);
            });
    }
}
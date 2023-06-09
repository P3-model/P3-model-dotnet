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
                var subProcesses = model.Relations
                    .OfType<Process.ContainsSubProcess>()
                    .Where(r => r.Parent == process)
                    .Select(r => r.Child);
                // TODO: recursive
                var steps = new HashSet<ProcessStep>(model.Relations
                    .OfType<Process.ContainsProcessStep>()
                    .Where(r => r.Process == process)
                    .Select(r => r.Step));
                var domainModules = model.Relations
                    .OfType<DomainModule.ContainsProcessStep>()
                    .Where(r => steps.Contains(r.ProcessStep))
                    .Select(r => r.DomainModule)
                    .Distinct()
                    .ToList();
                var deployableUnits = model.Relations
                    .OfType<CodeStructure.ImplementsProcessStep>()
                    .Where(r => steps.Contains(r.ProcessStep))
                    .SelectMany(r => model.Relations
                        .OfType<DeployableUnit.ContainsCodeStructure>()
                        .Where(r2 => r2.CodeStructure.Equals(r.CodeStructure))
                        .Select(r2 => r2.DeployableUnit))
                    .Distinct();
                var actors = model.Relations
                    .OfType<Actor.UsesProcessStep>()
                    .Where(r => steps.Contains(r.ProcessStep))
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
                return new ProcessPage(outputDirectory, process, parent, subProcesses, steps, domainModules,
                    deployableUnits, actors, developmentTeams, organizationalUnits);
            });
    }
}
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
public class ProcessStepPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        return model.Elements
            .OfType<ProcessStep>()
            .Select(step =>
            {
                var process = model.Relations
                    .OfType<Process.ContainsProcessStep>()
                    .Where(r => r.Step == step)
                    .Select(r => r.Process)
                    .Distinct()
                    // TODO: unique names across hierarchy
                    .FirstOrDefault();
                var domainModule = model.Relations
                    .OfType<DomainModule.ContainsProcessStep>()
                    .SingleOrDefault(r => r.ProcessStep == step)
                    ?.DomainModule;
                var deployableUnit = model.Relations
                    .OfType<CodeStructure.ImplementsProcessStep>()
                    .Where(r => r.ProcessStep == step)
                    .SelectMany(r => model.Relations
                        .OfType<DeployableUnit.ContainsCodeStructure>()
                        .Where(r2 => r2.CodeStructure == r.CodeStructure)
                        .Select(r2 => r2.DeployableUnit))
                    .Distinct()
                    .SingleOrDefault();
                var actors = model.Relations
                    .OfType<Actor.UsesProcessStep>()
                    .Where(r => r.ProcessStep == step)
                    .Select(r => r.Actor)
                    .Distinct();
                var developmentTeams = domainModule is null
                    ? Enumerable.Empty<DevelopmentTeam>()
                    : model.Relations
                        .OfType<DevelopmentTeam.OwnsDomainModule>()
                        .Where(r => r.DomainModule == domainModule)
                        .Select(r => r.Team)
                        .Distinct();
                var organizationalUnits = domainModule is null
                    ? Enumerable.Empty<BusinessOrganizationalUnit>()
                    : model.Relations
                        .OfType<BusinessOrganizationalUnit.OwnsDomainModule>()
                        .Where(r => r.DomainModule == domainModule)
                        .Select(r => r.Unit)
                        .Distinct();
                return new ProcessStepPage(outputDirectory, step, process, domainModule, deployableUnit, actors,
                    developmentTeams, organizationalUnits);
            });
    }
}
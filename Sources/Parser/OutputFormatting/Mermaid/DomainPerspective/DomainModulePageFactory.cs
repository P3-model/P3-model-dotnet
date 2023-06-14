using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

[UsedImplicitly]
public class DomainModulePageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        return model.Elements
            .OfType<DomainModule>()
            .Select(module =>
            {
                var parent = model.Relations
                    .OfType<DomainModule.ContainsDomainModule>()
                    .SingleOrDefault(r => r.Child == module)
                    ?.Parent;
                var children = new HashSet<DomainModule>(model.Relations
                    .OfType<DomainModule.ContainsDomainModule>()
                    .Where(r => r.Parent == module)
                    .Select(r => r.Child));
                var processes = model.Relations
                    .OfType<ProcessStep.BelongsToDomainModule>()
                    .Where(r => r.DomainModule.Equals(module))
                    .Select(r => r.Step)
                    .SelectMany(s => model.Relations
                        .OfType<Process.ContainsProcessStep>()
                        .Where(r2 => r2.Step.Equals(s))
                        .Select(r2 => r2.Process))
                    .Distinct();
                var deployableUnits = model.Relations
                    .OfType<DomainModule.IsDeployedInDeployableUnit>()
                    .Where(r => r.DomainModule.Equals(module))
                    .Select(r => r.DeployableUnit)
                    .Distinct();
                // TODO: relation to owners from all hierarchy levels
                var developmentTeams = model.Relations
                    .OfType<DevelopmentTeam.OwnsDomainModule>()
                    .Where(r => r.DomainModule == module)
                    .Select(r => r.Team)
                    .Distinct();
                var organizationalUnits = model.Relations
                    .OfType<BusinessOrganizationalUnit.OwnsDomainModule>()
                    .Where(r => r.DomainModule == module)
                    .Select(r => r.Unit)
                    .Distinct();
                return new DomainModulePage(outputDirectory, module, parent, children, processes, deployableUnits, 
                    developmentTeams, organizationalUnits);
            });
    }
}
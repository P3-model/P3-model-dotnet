using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

public class DeployableUnitPage : MermaidPageBase
{
    private readonly DeployableUnit _unit;
    private readonly IEnumerable<DomainModule> _modules;
    private readonly IEnumerable<DevelopmentTeam> _teams;

    public DeployableUnitPage(string outputDirectory, DeployableUnit unit, IEnumerable<DomainModule> modules, 
        IEnumerable<DevelopmentTeam> teams) : base(outputDirectory)
    {
        _unit = unit;
        _modules = modules;
        _teams = teams;
    }

    public override string Header => $"[*Deployable unit*] {_unit.Name}";
    protected override string Description => @$"This view contains details information about {_unit.Name} deployable unit, including:
- related domain modules
- related development teams";

    public override string RelativeFilePath => $"DeployableUnits/{_unit.Name}.md";

    public override Element MainElement => _unit;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related domain modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var unitId = flowchartWriter.WriteRectangle(_unit.Name);
            foreach (var module in _modules)
            {
                var moduleId = flowchartWriter.WriteStadiumShape(module.Name);
                flowchartWriter.WriteArrow(unitId, moduleId);
            }
            
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Related development teams", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var unitId = flowchartWriter.WriteRectangle(_unit.Name);
            foreach (var team in _teams)
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name);
                flowchartWriter.WriteArrow(unitId, teamId);
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is DeployableUnitsPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
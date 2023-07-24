using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

public class DeployableUnitPage : MermaidPageBase
{
    private readonly DeployableUnit _unit;
    private readonly Tier? _tier;
    private readonly IEnumerable<DomainModule> _modules;
    private readonly IEnumerable<DevelopmentTeam> _teams;

    public DeployableUnitPage(string outputDirectory, DeployableUnit unit, Tier? tier,
        IEnumerable<DomainModule> modules, IEnumerable<DevelopmentTeam> teams) : base(outputDirectory)
    {
        _unit = unit;
        _tier = tier;
        _modules = modules;
        _teams = teams;
    }

    protected override string Description =>
        @$"This view contains details information about {_unit.Name} deployable unit, including:
- related domain modules
- related development teams";

    public override string RelativeFilePath => Path.Combine("Technology", "DeployableUnits", $"{_unit.Name}.md");
    public override Element? MainElement => _unit;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related domain modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            if (_tier != null)
                flowchartWriter.WriteSubgraph(_tier.Name, WriteUnitWithModules);
            else
                WriteUnitWithModules(flowchartWriter);
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Related development teams", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
            foreach (var team in _teams.OrderBy(t => t.Name))
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(teamId, unitId, "maintains");
            }
        });
    }

    private void WriteUnitWithModules(FlowchartElementsWriter flowchartWriter)
    {
        var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
        foreach (var module in _modules.OrderBy(m => m.Name))
        {
            var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
            flowchartWriter.WriteArrow(moduleId, unitId, "is deployed in");
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is DeployableUnitsPage;
}
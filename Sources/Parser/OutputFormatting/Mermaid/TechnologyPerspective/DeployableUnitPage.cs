using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;
using P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

public class DeployableUnitPage : MermaidPageBase
{
    private readonly DeployableUnit _unit;
    private readonly Tier? _tier;
    private readonly IReadOnlySet<CSharpProject> _cSharpProjects;
    private readonly IReadOnlySet<DomainModule> _modules;
    private readonly IReadOnlySet<DevelopmentTeam> _teams;

    public DeployableUnitPage(string outputDirectory, DeployableUnit unit, Tier? tier,
        IReadOnlySet<CSharpProject> cSharpProjects, IReadOnlySet<DomainModule> modules, 
        IReadOnlySet<DevelopmentTeam> teams) : base(outputDirectory)
    {
        _unit = unit;
        _tier = tier;
        _modules = modules;
        _teams = teams;
        _cSharpProjects = cSharpProjects;
    }

    protected override string Description =>
        @$"This view contains details information about {_unit.Name} deployable unit, including:
- related domain modules
- related development teams";

    public override string RelativeFilePath => Path.Combine("Technology", "DeployableUnits", 
        $"{_unit.Name.Dehumanize()}.md");
    public override Element MainElement => _unit;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related domain modules", 3);
        if (_modules.Count == 0)
        {
            mermaidWriter.WriteLine("No related modules were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
                foreach (var module in _modules.OrderBy(m => m.Name))
                {
                    var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(moduleId, unitId, "is deployed in");
                }
            });
        }
        
        mermaidWriter.WriteHeading("Technology Perspective", 2);
        if (_tier != null)
            mermaidWriter.WriteHeading($"Tier: {_tier.Name}", 3);
        mermaidWriter.WriteHeading("Related c# projects", 3);
        if (_cSharpProjects.Count == 0)
        {
            mermaidWriter.WriteLine("No related c# projects were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
                foreach (var project in _cSharpProjects.OrderBy(t => t.Name))
                {
                    var projectId = flowchartWriter.WriteStadiumShape(project.Name, Style.TechnologyPerspective);
                    flowchartWriter.WriteArrow(unitId, projectId, "contains");
                }
                
            });
        }

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Related development teams", 3);
        if (_teams.Count == 0)
        {
            mermaidWriter.WriteLine("No related development teams were found.");
        }
        else
        {
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
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => _modules.Contains(modulePage.MainElement),
        DevelopmentTeamPage teamPage => _teams.Contains(teamPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is DeployableUnitsPage;
}
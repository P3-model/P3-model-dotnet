using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.OutputFormatting.Mermaid.Domain;
using P3Model.Parser.OutputFormatting.Mermaid.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.People;

public class DevelopmentTeamPage : MermaidPageBase
{
    private readonly DevelopmentTeam _team;
    private readonly IReadOnlySet<DomainModule> _modules;
    private readonly IReadOnlySet<DeployableUnit> _deployableUnits;

    public DevelopmentTeamPage(string outputDirectory, DevelopmentTeam team, IReadOnlySet<DomainModule> modules, 
        IReadOnlySet<DeployableUnit> deployableUnits) : base(outputDirectory)
    {
        _team = team;
        _modules = modules;
        _deployableUnits = deployableUnits;
    }

    protected override string Description => @$"This view contains details information about {_team.Name} team, including:
- related domain modules
- related deployable units";

    public override string RelativeFilePath => Path.Combine("People", "DevelopmentTeams", 
        $"{_team.Name.Dehumanize()}.md");

    public override ElementBase MainElement => _team;

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
                var teamId = flowchartWriter.WriteRectangle(_team.Name, Style.PeoplePerspective);
                foreach (var module in _modules.OrderBy(m => m.Name))
                {
                    var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(teamId, moduleId, "develops & maintains");
                }
            
            });
        }

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Related deployable units", 3);
        if (_deployableUnits.Count == 0)
        {
            mermaidWriter.WriteLine("No related deployable units were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var teamId = flowchartWriter.WriteRectangle(_team.Name, Style.PeoplePerspective);
                foreach (var deployableUnit in _deployableUnits.OrderBy(u => u.Name))
                {
                    var deployableUnitId = flowchartWriter.WriteStadiumShape(deployableUnit.Name, Style.TechnologyPerspective);
                    flowchartWriter.WriteArrow(teamId, deployableUnitId, "maintains");
                }
            });
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => _modules.Contains(modulePage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnits.Contains(deployableUnitPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is DevelopmentTeamsPage or
        DomainModuleOwnersPage;
}
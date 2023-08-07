using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

public class DevelopmentTeamsPage : MermaidPageBase
{
    private readonly DocumentedSystem _system;
    private readonly IReadOnlySet<DevelopmentTeam> _teams;

    public DevelopmentTeamsPage(string outputDirectory, DocumentedSystem system, IReadOnlySet<DevelopmentTeam> teams)
        : base(outputDirectory)
    {
        _system = system;
        _teams = teams;
    }

    public override string Header => "Development Teams";

    protected override string Description =>
        $"This view contains all development teams that build and maintain {_system.Name} product.";

    public override string RelativeFilePath => Path.Combine("People", "DevelopmentTeams", "DevelopmentTeams.md");
    public override Element? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.People;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Teams and their relations", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            if (_teams.Count == 0)
            {
                flowchartWriter.WriteStadiumShape("no teams found");
            }
            else
            {
                foreach (var team in _teams.OrderBy(t => t.Name))
                    flowchartWriter.WriteRectangle(team.Name, Style.PeoplePerspective);
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DevelopmentTeamPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
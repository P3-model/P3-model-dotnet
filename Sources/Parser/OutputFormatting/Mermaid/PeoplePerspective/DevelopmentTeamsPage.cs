using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

public class DevelopmentTeamsPage : MermaidPageBase
{
    private readonly DocumentedSystem _system;
    private readonly IEnumerable<DevelopmentTeam> _teams;

    public DevelopmentTeamsPage(string outputDirectory, DocumentedSystem system, IEnumerable<DevelopmentTeam> teams)
        : base(outputDirectory)
    {
        _system = system;
        _teams = teams;
    }

    public override string Header => "Development teams";

    protected override string Description =>
        $"This view contains all development teams that build and maintain {_system.Name} product.";

    public override string RelativeFilePath => "Development_Teams.md";
    public override Element? MainElement => null;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Teams and their relations", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var team in _teams.OrderBy(t => t.Name))
                flowchartWriter.WriteRectangle(team.Name, Style.PeoplePerspective);
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DevelopmentTeamPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

public class DevelopmentTeamsPage : MermaidPageBase
{
    private readonly Product _product;
    private readonly IEnumerable<DevelopmentTeam> _teams;

    public DevelopmentTeamsPage(string outputDirectory, Product product, IEnumerable<DevelopmentTeam> teams)
        : base(outputDirectory)
    {
        _product = product;
        _teams = teams;
    }

    public override string Header => "Development teams";

    protected override string Description =>
        $"This view contains all development teams that build and maintain {_product.Name} product.";

    public override string RelativeFilePath => "Development_Teams.md";
    public override Element MainElement => _product;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Teams and their relations", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var team in _teams)
                flowchartWriter.WriteRectangle(team.Name);
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DevelopmentTeamPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
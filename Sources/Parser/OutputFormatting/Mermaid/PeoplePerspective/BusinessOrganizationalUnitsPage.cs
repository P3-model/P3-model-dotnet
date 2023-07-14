using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

public class BusinessOrganizationalUnitsPage : MermaidPageBase
{
    private readonly ModelSyntax.DocumentedSystem _system;
    private readonly IEnumerable<BusinessOrganizationalUnit> _organizationalUnits;

    public BusinessOrganizationalUnitsPage(string outputDirectory, ModelSyntax.DocumentedSystem system,
        IEnumerable<BusinessOrganizationalUnit> organizationalUnits) : base(outputDirectory)
    {
        _system = system;
        _organizationalUnits = organizationalUnits;
    }

    public override string Header => "Business organizational units";

    protected override string Description =>
        $"This view contains all business organizational units that owns {_system.Name} product";

    public override string RelativeFilePath => "Business_Organizational_Units.md";
    public override Element? MainElement => null;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Units structure", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var organizationalUnit in _organizationalUnits.OrderBy(u => u.Name))
                flowchartWriter.WriteRectangle(organizationalUnit.Name, Style.PeoplePerspective);
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is BusinessOrganizationalUnitPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
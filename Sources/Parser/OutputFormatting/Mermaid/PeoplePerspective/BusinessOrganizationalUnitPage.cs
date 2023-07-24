using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

public class BusinessOrganizationalUnitPage : MermaidPageBase
{
    private readonly BusinessOrganizationalUnit _organizationalUnit;
    private readonly IEnumerable<DomainModule> _modules;

    public BusinessOrganizationalUnitPage(string outputDirectory, BusinessOrganizationalUnit organizationalUnit, 
        IEnumerable<DomainModule> modules) : base(outputDirectory)
    {
        _organizationalUnit = organizationalUnit;
        _modules = modules;
    }
    
    protected override string Description => @$"This view contains details information about {_organizationalUnit.Name}, including:
- related domain modules";

    public override string RelativeFilePath => Path.Combine(
        "People", "BusinessOrganizationalUnits", $"{_organizationalUnit.Name}.md");

    public override Element? MainElement => _organizationalUnit;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related domain modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var unitId = flowchartWriter.WriteRectangle(_organizationalUnit.Name, Style.PeoplePerspective);
            foreach (var module in _modules.OrderBy(m => m.Name))
            {
                var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(unitId, moduleId, "owns");
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is BusinessOrganizationalUnitsPage;
}
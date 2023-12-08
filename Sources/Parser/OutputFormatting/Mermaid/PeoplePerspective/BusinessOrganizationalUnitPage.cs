using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

public class BusinessOrganizationalUnitPage : MermaidPageBase
{
    private readonly BusinessOrganizationalUnit _organizationalUnit;
    private readonly IReadOnlySet<DomainModule> _modules;

    public BusinessOrganizationalUnitPage(string outputDirectory, BusinessOrganizationalUnit organizationalUnit,
        IReadOnlySet<DomainModule> modules) : base(outputDirectory)
    {
        _organizationalUnit = organizationalUnit;
        _modules = modules;
    }

    protected override string Description =>
        @$"This view contains details information about {_organizationalUnit.Name}, including:
- related domain modules";

    public override string RelativeFilePath => Path.Combine(
        "People", "BusinessOrganizationalUnits", $"{_organizationalUnit.Name.Dehumanize()}.md");

    public override ElementBase MainElement => _organizationalUnit;

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
                var unitId = flowchartWriter.WriteRectangle(_organizationalUnit.Name, Style.PeoplePerspective);
                foreach (var module in _modules.OrderBy(m => m.Name))
                {
                    var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(unitId, moduleId, "owns");
                }
            });
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => _modules.Contains(modulePage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is BusinessOrganizationalUnitsPage or
        DomainModuleOwnersPage;
}
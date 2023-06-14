using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

public class DeployableUnitsPage : MermaidPageBase
{
    private readonly Product _product;
    private readonly IEnumerable<DeployableUnit> _units;

    public DeployableUnitsPage(string outputDirectory, Product product, IEnumerable<DeployableUnit> units)
        : base(outputDirectory)
    {
        _product = product;
        _units = units;
    }

    public override string Header => "Deployable units";

    protected override string Description =>
        $"This view contains all deployable units for {_product.Name} product.";

    public override string RelativeFilePath => "Deployable_Units.md";
    public override Element MainElement => _product;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Deployable units and their tires", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var unit in _units)
                flowchartWriter.WriteRectangle(unit.Name);
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DeployableUnitPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
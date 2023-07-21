using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

public class DeployableUnitsPage : MermaidPageBase
{
    private readonly DocumentedSystem _system;
    private readonly IEnumerable<DeployableUnit> _unitsWithoutTier;
    private readonly IEnumerable<Tier.ContainsDeployableUnit> _tierContainsDeploymentUnitRelations;

    public DeployableUnitsPage(string outputDirectory, DocumentedSystem system, IEnumerable<DeployableUnit> units,
        IEnumerable<Tier.ContainsDeployableUnit> tierContainsDeploymentUnitRelations)
        : base(outputDirectory)
    {
        _system = system;
        _unitsWithoutTier = units
            .Where(u => tierContainsDeploymentUnitRelations
                .All(r => !r.Destination.Equals(u)));
        _tierContainsDeploymentUnitRelations = tierContainsDeploymentUnitRelations;
    }

    public override string Header => "Deployable units";
    protected override string Description => $"This view contains all deployable units for {_system.Name} product.";
    public override string RelativeFilePath => "Deployable_Units.md";
    public override Element? MainElement => null;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Deployable units and their tires", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var relation in _tierContainsDeploymentUnitRelations
                         .OrderBy(r => r.Source.Name)
                         .ThenBy(r => r.Destination.Name))
                flowchartWriter.WriteSubgraph(relation.Source.Name, elementsWriter => elementsWriter
                    .WriteRectangle(relation.Destination.Name, Style.TechnologyPerspective));

            foreach (var unit in _unitsWithoutTier.OrderBy(u => u.Name))
                flowchartWriter.WriteRectangle(unit.Name, Style.TechnologyPerspective);
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DeployableUnitPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
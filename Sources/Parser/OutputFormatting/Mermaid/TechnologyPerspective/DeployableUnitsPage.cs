using System.Collections.Generic;
using System.IO;
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

    public override string Header => "Deployable Units";
    protected override string Description => $"This view contains all deployable units for {_system.Name} product.";
    public override string RelativeFilePath => Path.Combine("Technology", "DeployableUnits", "DeployableUnits.md");
    public override ElementBase? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.Technology;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Deployable units and their tires", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var group in _tierContainsDeploymentUnitRelations
                         .GroupBy(r => r.Source)
                         .OrderBy(r => r.Key.Name))
            {
                flowchartWriter.WriteSubgraph(group.Key.Name, subGraphWriter =>
                {
                    foreach (var relation in group)
                        subGraphWriter.WriteRectangle(relation.Destination.Name, Style.TechnologyPerspective);
                });
            }
            foreach (var unit in _unitsWithoutTier.OrderBy(u => u.Name))
                flowchartWriter.WriteRectangle(unit.Name, Style.TechnologyPerspective);
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DeployableUnitPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
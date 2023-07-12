using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainBuildingBlockPage : MermaidPageBase
{
    private readonly DomainBuildingBlock _buildingBlock;
    private readonly DomainModule? _module;
    private readonly IReadOnlySet<(DomainBuildingBlock BuildingBlock, DomainModule? Module)> _dependencies;
    private readonly IReadOnlySet<ProcessStep> _processSteps;

    public DomainBuildingBlockPage(string outputDirectory, DomainBuildingBlock buildingBlock, DomainModule? module,
        IReadOnlySet<(DomainBuildingBlock, DomainModule?)> dependencies,
        IReadOnlySet<ProcessStep> processSteps) : base(outputDirectory)
    {
        _buildingBlock = buildingBlock;
        _module = module;
        _dependencies = dependencies;
        _processSteps = processSteps;
    }

    public override string Header => $"[*Domain building block*] {_buildingBlock.Name}";

    protected override string Description =>
        @$"This view contains details information about {_buildingBlock.Name} building block, including:
- dependencies
- modules
- related processes";

    public override string RelativeFilePath => _module is null
        ? Path.Combine("BuildingBlocks", $"/{_buildingBlock.Name}.md")
        : Path.Combine("BuildingBlocks", Path.Combine(_module.Id.Parts.ToArray()), $"{_buildingBlock.Name}.md");

    public override Element MainElement => _buildingBlock;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Dependencies", 3);

        if (_dependencies.Count == 0)
        {
            mermaidWriter.WriteLine($"{_buildingBlock.Name} has no dependencies.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var buildingBlockId = _module is null
                    ? flowchartWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective)
                    : flowchartWriter.WriteSubgraph(_module.Name, subGraphWriter => subGraphWriter
                        .WriteRectangle(_buildingBlock.Name, Style.DomainPerspective));
                var dependencyIds = new List<string>();
                foreach (var group in _dependencies.GroupBy(t => t.Module, t => t.BuildingBlock))
                {
                    if (group.Key is null)
                        dependencyIds.AddRange(WriteDependencies(group, flowchartWriter));
                    else
                        dependencyIds.Add(flowchartWriter.WriteSubgraph(group.Key.Name, subgraphWriter =>
                            WriteDependencies(group, subgraphWriter)));
                }
                foreach (var dependencyId in dependencyIds)
                    flowchartWriter.WriteArrow(buildingBlockId, dependencyId, "depends on");
            });
        }
        mermaidWriter.WriteHeading("Related process steps", 3);
        if (_processSteps.Count == 0)
        {
            mermaidWriter.WriteLine($"{_buildingBlock.Name} is not used in any process step.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var buildingBlockId = flowchartWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective);
                foreach (var processStep in _processSteps)
                {
                    var processStepId = flowchartWriter.WriteStadiumShape(processStep.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(buildingBlockId, processStepId, "is used in");
                }
            });
        }
    }

    private static IEnumerable<string> WriteDependencies(IEnumerable<DomainBuildingBlock> dependencies,
        FlowchartElementsWriter flowchartWriter) => dependencies
        .Select(dependency => flowchartWriter.WriteStadiumShape(dependency.Name, Style.DomainPerspective))
        .ToList();

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) =>
        page is DomainModulePage modulePage && modulePage.MainElement.Equals(_module);

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) =>
        page is DomainBuildingBlockPage buildingBlockPage &&
        _dependencies
            .Select(d => d.BuildingBlock)
            .Contains((DomainBuildingBlock)buildingBlockPage.MainElement);
}
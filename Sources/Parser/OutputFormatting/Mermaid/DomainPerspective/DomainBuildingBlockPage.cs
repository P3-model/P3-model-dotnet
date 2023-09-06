using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainBuildingBlockPage : MermaidPageBase
{
    private readonly DomainBuildingBlock _buildingBlock;
    private readonly DomainModule? _module;
    private readonly IReadOnlySet<(DomainBuildingBlock BuildingBlock, DomainModule? Module)> _usingElements;
    private readonly IReadOnlySet<(DomainBuildingBlock BuildingBlock, DomainModule? Module)> _usedElements;

    public DomainBuildingBlockPage(string outputDirectory, DomainBuildingBlock buildingBlock, DomainModule? module,
        IReadOnlySet<(DomainBuildingBlock, DomainModule?)> usingElements,
        IReadOnlySet<(DomainBuildingBlock, DomainModule?)> usedElements) : base(outputDirectory)
    {
        _buildingBlock = buildingBlock;
        _module = module;
        _usingElements = usingElements;
        _usedElements = usedElements;
    }

    protected override string Description =>
        @$"This view contains details information about {_buildingBlock.Name} building block, including:
- dependencies
- modules
- related processes";

    public override string RelativeFilePath => _module is null
        ? Path.Combine("Domain", "Concepts", $"{_buildingBlock.Name.Dehumanize()}.md")
        : Path.Combine("Domain", "Concepts", Path.Combine(_module.Id.Parts.ToArray()),
            $"{_buildingBlock.Name.Dehumanize()}.md");

    public override Element MainElement => _buildingBlock;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Dependencies", 3);

        if (_usingElements.Count == 0 && _usedElements.Count == 0)
        {
            mermaidWriter.WriteLine("No dependencies were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var usingElementIds = new List<string>();
                foreach (var group in _usingElements
                             .GroupBy(t => t.Module, t => t.BuildingBlock)
                             .OrderBy(t => t.Key is null ? string.Empty : t.Key.FullName))
                {
                    if (group.Key is null)
                        usingElementIds.AddRange(WriteDependencies(group, flowchartWriter));
                    else
                        usingElementIds.Add(flowchartWriter.WriteSubgraph(group.Key.FullName, subgraphWriter =>
                            WriteDependencies(group, subgraphWriter)));
                }
                var buildingBlockId = _module is null
                    ? flowchartWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective)
                    : flowchartWriter.WriteSubgraph(_module.FullName, subGraphWriter => subGraphWriter
                        .WriteRectangle(_buildingBlock.Name, Style.DomainPerspective));
                var usedElementIds = new List<string>();
                foreach (var group in _usedElements
                             .GroupBy(t => t.Module, t => t.BuildingBlock)
                             .OrderBy(t => t.Key is null ? string.Empty : t.Key.FullName))
                {
                    if (group.Key is null)
                        usedElementIds.AddRange(WriteDependencies(group, flowchartWriter));
                    else
                        usedElementIds.Add(flowchartWriter.WriteSubgraph(group.Key.FullName, subgraphWriter =>
                            WriteDependencies(group, subgraphWriter)));
                }
                foreach (var usingElementId in usingElementIds)
                    flowchartWriter.WriteArrow(usingElementId, buildingBlockId, "depends on");
                foreach (var usedElementId in usedElementIds)
                    flowchartWriter.WriteArrow(buildingBlockId, usedElementId, "depends on");
            });
        }
        mermaidWriter.WriteHeading("Related process steps", 3);
        var processSteps = _usingElements
            .Select(e => e.BuildingBlock)
            .OfType<ProcessStep>()
            .OrderBy(step => step.Module is null ? string.Empty : step.Module.FullName)
            .ThenBy(step => step.Name)
            .ToList();
        if (processSteps.Count == 0)
        {
            mermaidWriter.WriteLine("No related processes were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var buildingBlockId = flowchartWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective);
                foreach (var processStep in processSteps)
                {
                    var processStepId = flowchartWriter.WriteStadiumShape(processStep.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(buildingBlockId, processStepId, "is used in");
                }
            });
        }
    }

    private static IEnumerable<string> WriteDependencies(IEnumerable<DomainBuildingBlock> dependencies,
        FlowchartElementsWriter flowchartWriter) => dependencies
        .OrderBy(dependency => dependency.Name)
        .Select(dependency => flowchartWriter.WriteStadiumShape(dependency.Name, Style.DomainPerspective))
        .ToList();

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainBuildingBlockPage buildingBlockPage => _usedElements
            .Select(e => e.BuildingBlock)
            .Contains(buildingBlockPage.MainElement),
        ProcessStepPage processStepPage => _usingElements
            .Select(e => e.BuildingBlock)
            .OfType<ProcessStep>()
            .Contains(processStepPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) =>
        page is DomainModulePage modulePage && modulePage.MainElement!.Equals(_module);
}
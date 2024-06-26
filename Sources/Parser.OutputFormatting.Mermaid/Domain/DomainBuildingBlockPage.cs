using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.Domain;

public class DomainBuildingBlockPage : MermaidPageBase
{
    private readonly DomainBuildingBlock _buildingBlock;
    private readonly DomainModule? _module;
    private readonly IReadOnlySet<(DomainBuildingBlock BuildingBlock, DomainModule? Module)> _usingElements;
    private readonly IReadOnlySet<(DomainBuildingBlock BuildingBlock, DomainModule? Module)> _usedElements;
    private readonly IReadOnlySet<CodeStructure> _codeStructures;

    public DomainBuildingBlockPage(string outputDirectory, DomainBuildingBlock buildingBlock, DomainModule? module,
        IReadOnlySet<(DomainBuildingBlock, DomainModule?)> usingElements,
        IReadOnlySet<(DomainBuildingBlock, DomainModule?)> usedElements,
        IReadOnlySet<CodeStructure> codeStructures) : base(outputDirectory)
    {
        _buildingBlock = buildingBlock;
        _module = module;
        _usingElements = usingElements;
        _usedElements = usedElements;
        _codeStructures = codeStructures;
    }

    protected override string Description =>
        @$"This view contains details information about {_buildingBlock.Name} building block, including:
- dependencies
- modules
- related processes";

    public override string RelativeFilePath => _module is null
        ? Path.Combine("Domain", "Modules", $"{_buildingBlock.Name.Dehumanize()}.md")
        : Path.Combine("Domain",
            "Modules",
            Path.Combine(_module.HierarchyPath.Parts.ToArray()),
            $"{_buildingBlock.Name.Dehumanize()}.md");

    public override ElementBase MainElement => _buildingBlock;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        if (_buildingBlock.ShortDescription != null)
        {
            mermaidWriter.WriteHeading("User defined description", 2);
            mermaidWriter.WriteLine(_buildingBlock.ShortDescription);
        }

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
                             .OrderBy(t => t.Key is null ? string.Empty : t.Key.HierarchyPath.Full))
                {
                    if (group.Key is null)
                        usingElementIds.AddRange(WriteDependencies(group, flowchartWriter));
                    else
                        usingElementIds.Add(flowchartWriter.WriteSubgraph(group.Key.HierarchyPath.Full,
                            subgraphWriter => WriteDependencies(group, subgraphWriter)));
                }
                var buildingBlockId = _module is null
                    ? flowchartWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective)
                    : flowchartWriter.WriteSubgraph(_module.HierarchyPath.Full,
                        subGraphWriter => subGraphWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective));
                var usedElementIds = new List<string>();
                foreach (var group in _usedElements
                             .GroupBy(t => t.Module, t => t.BuildingBlock)
                             .OrderBy(t => t.Key is null ? string.Empty : t.Key.HierarchyPath.Full))
                {
                    if (group.Key is null)
                        usedElementIds.AddRange(WriteDependencies(group, flowchartWriter));
                    else
                        usedElementIds.Add(flowchartWriter.WriteSubgraph(group.Key.HierarchyPath.Full,
                            subgraphWriter => WriteDependencies(group, subgraphWriter)));
                }
                foreach (var usingElementId in usingElementIds)
                    flowchartWriter.WriteArrow(usingElementId, buildingBlockId, "depends on");
                foreach (var usedElementId in usedElementIds)
                    flowchartWriter.WriteArrow(buildingBlockId, usedElementId, "depends on");
            });
        }
        mermaidWriter.WriteHeading("Related use cases", 3);
        var useCases = _usingElements
            .Select(e => e.BuildingBlock)
            .OfType<UseCase>()
            .OrderBy(useCase => useCase.Id)
            .ThenBy(useCase => useCase.Name)
            .ToList();
        if (useCases.Count == 0)
        {
            mermaidWriter.WriteLine("No related processes were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var buildingBlockId = flowchartWriter.WriteRectangle(_buildingBlock.Name, Style.DomainPerspective);
                foreach (var useCase in useCases)
                {
                    var useCaseId = flowchartWriter.WriteStadiumShape(useCase.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(buildingBlockId, useCaseId, "is used in");
                }
            });
        }

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Source code", 3);
        var sourceCodePaths = _codeStructures
            .Where(s => !string.IsNullOrWhiteSpace(s.SourceCodePath))
            .Select(s => s.SourceCodePath)
            .Distinct()
            .OrderBy(p => p)
            .ToList();
        switch (sourceCodePaths.Count)
        {
            case 0:
                mermaidWriter.WriteLine("No source code files were found.");
                break;
            case 1:
                mermaidWriter.WriteLine(FormatSourceCodeLink(sourceCodePaths[0]));
                break;
            default:
                mermaidWriter.WriteUnorderedList(sourceCodePaths, FormatSourceCodeLink);
                break;
        }
    }

    private string FormatSourceCodeLink(string path) => MermaidWriter
        .FormatLink(Path.GetFileName(path), GetPathRelativeToPageFile(path));

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
        UseCasePage useCasePage => _usingElements
            .Select(e => e.BuildingBlock)
            .OfType<UseCase>()
            .Contains(useCasePage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) =>
        page is DomainModulePage modulePage && modulePage.MainElement!.Equals(_module);
}
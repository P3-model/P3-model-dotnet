using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainBuildingBlocksPage : MermaidPageBase
{
    private readonly DomainModulesHierarchy _modulesHierarchy;
    private readonly DomainBuildingBlocks _buildingBlocks;

    public override string Header => "Domain Building Blocks";
    public override string RelativeFilePath => "BuildingBlocks.md";

    public override Element MainElement => null;

    public DomainBuildingBlocksPage(string outputDirectory,
        DomainModulesHierarchy modulesHierarchy,
        DomainBuildingBlocks buildingBlocks) 
        : base(outputDirectory)
    {
        _modulesHierarchy = modulesHierarchy;
        _buildingBlocks = buildingBlocks;
    }

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Building Blocks", 1);
        foreach (var module in _modulesHierarchy
                     .FromLevel(0)
                     .OrderBy(m => m.Name))
        {
            mermaidWriter.WriteHeading($"{module.Name}", 2);
            mermaidWriter.WriteFlowchart(flowchartWriter => Write(module, flowchartWriter));
        }
    }

    private void Write(DomainModule module, FlowchartElementsWriter flowchartWriter) => flowchartWriter
        .WriteSubgraph($"{module.Name}", subgraphWriter =>
        {
            foreach (var child in _modulesHierarchy.GetChildrenFor(module).OrderBy(r => r.Name))
                Write(child, subgraphWriter);
            foreach (var buildingBlock in _buildingBlocks.GetFor(module).OrderBy(r => r.Name))
                subgraphWriter.WriteStadiumShape(
                    @$"""<i>{buildingBlock.GetType().Name}</i>\n<b>{buildingBlock.Name}</b>""");
        });

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
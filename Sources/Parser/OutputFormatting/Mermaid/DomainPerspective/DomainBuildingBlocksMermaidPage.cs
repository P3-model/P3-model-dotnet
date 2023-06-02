using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainBuildingBlocksMermaidPage : MermaidPage
{
    public string RelativeFilePath => "BuildingBlocks.md";

    public void Write(Model model, MermaidWriter mermaidWriter, MermaidPages allPages)
    {
        mermaidWriter.WriteHeading("Building Blocks", 1);
        foreach (var module in model.Elements
                     .OfType<DomainModule>()
                     .Where(m => m.Level == 0)
                     .OrderBy(m => m.Name))
        {
            mermaidWriter.WriteHeading($"{module.Name}", 2);
            mermaidWriter.WriteFlowchart(flowchartWriter => Write(module, model, flowchartWriter));
        }
    }

    private static void Write(DomainModule module, Model model, FlowchartElementsWriter flowchartWriter)
    {
        flowchartWriter.WriteSubgraph($"{module.Name}", subgraphWriter =>
        {
            foreach (var relation in model.Relations
                         .OfType<DomainModule.ContainsDomainModule>()
                         .Where(r => r.Parent == module)
                         .OrderBy(r => r.Parent.Name)
                         .ThenBy(r => r.Child.Name)) 
                Write(relation.Child, model, subgraphWriter);
            foreach (var relation in model.Relations
                         .OfType<ModuleContainsBuildingBlock>()
                         .Where(r => r.DomainModule == module)
                         .OrderBy(r => r.DomainModule.Name)
                         .ThenBy(r => r.BuildingBlock.Name))
                subgraphWriter.WriteStadiumShape(@$"""<i>{relation.BuildingBlock.GetType().Name}</i>\n<b>{relation.BuildingBlock.Name}</b>""");
        });
    }
}
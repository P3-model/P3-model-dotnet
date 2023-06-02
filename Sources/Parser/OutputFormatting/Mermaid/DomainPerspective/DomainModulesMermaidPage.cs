using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainModulesMermaidPage : MermaidPage
{
    public string RelativeFilePath => "Modules.md";

    public void Write(Model model, MermaidWriter mermaidWriter, MermaidPages allPages)
    {
        mermaidWriter.WriteHeading("Modules", 1);
        foreach (var module in model.Elements
                     .OfType<DomainModule>()
                     .Where(m => m.Level == 0)
                     .OrderBy(m => m.Name))
        {
            mermaidWriter.WriteHeading($"{module.Name}", 2);
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var moduleId = flowchartWriter.WriteRectangle(module.Name);
                Write(module, moduleId, model, flowchartWriter);
            });
        }
    }
    
    private static void Write(DomainModule parent, int parentId, Model model, FlowchartElementsWriter flowchartWriter)
    {
        foreach (var relation in model.Relations
                     .OfType<DomainModule.ContainsDomainModule>()
                     .Where(r => r.Parent == parent)
                     .OrderBy(r => r.Parent.Name)
                     .ThenBy(r => r.Child.Name))
        {
            var childId = flowchartWriter.WriteRectangle(relation.Child.Name);
            flowchartWriter.WriteArrow(parentId, childId);
            Write(relation.Child, childId, model, flowchartWriter);
        }
    }
}
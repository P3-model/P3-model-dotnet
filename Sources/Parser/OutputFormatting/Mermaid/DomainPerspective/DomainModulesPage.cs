using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainModulesPage : MermaidPageBase
{
    private readonly DomainModulesHierarchy _modulesHierarchy;

    public override string Header => "Domain Modules";
    
    public override string RelativeFilePath => "Modules.md";

    public override Element MainElement => null;

    public DomainModulesPage(string outputDirectory, DomainModulesHierarchy modulesHierarchy) 
        : base(outputDirectory) =>
        _modulesHierarchy = modulesHierarchy;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Modules", 1);
        foreach (var module in _modulesHierarchy.FromLevel(0).OrderBy(m => m.Name))
        {
            mermaidWriter.WriteHeading($"{module.Name}", 2);
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var moduleId = flowchartWriter.WriteRectangle(module.Name);
                Write(module, moduleId, flowchartWriter);
            });
        }
    }

    private void Write(DomainModule parent, int parentId, FlowchartElementsWriter flowchartWriter)
    {
        foreach (var child in _modulesHierarchy.GetChildrenFor(parent).OrderBy(r => r.Name))
        {
            var childId = flowchartWriter.WriteRectangle(child.Name);
            flowchartWriter.WriteArrow(parentId, childId);
            Write(child, childId, flowchartWriter);
        }
    }
    
    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
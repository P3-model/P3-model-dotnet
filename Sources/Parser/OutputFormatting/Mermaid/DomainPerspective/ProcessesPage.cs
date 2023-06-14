using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class ProcessesPage : MermaidPageBase
{
    private readonly Product _product;
    private readonly ProcessesHierarchy _processesHierarchy;

    public ProcessesPage(string outputDirectory, Product product, ProcessesHierarchy processesHierarchy)
        : base(outputDirectory)
    {
        _product = product;
        _processesHierarchy = processesHierarchy;
    }

    public override string Header => "Business processes";
    protected override string Description => "This view contains all business processes with their sub-processes.";
    public override string RelativeFilePath => "Business_Processes.md";
    public override Element MainElement => _product;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        foreach (var process in _processesHierarchy.FromFirstLevel())
        {
            mermaidWriter.WriteHeading(process.Name, 2);
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var processId = flowchartWriter.WriteRectangle(process.Name, Style.DomainPerspective);
                Write(process, processId, flowchartWriter);
            });
        }
    }

    private void Write(Process parent, int parentId, FlowchartElementsWriter flowchartWriter)
    {
        foreach (var child in _processesHierarchy.GetChildrenFor(parent).OrderBy(r => r.Name))
        {
            var childId = flowchartWriter.WriteRectangle(child.Name, Style.DomainPerspective);
            flowchartWriter.WriteArrow(parentId, childId, "contains");
            Write(child, childId, flowchartWriter);
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is ProcessPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
    
    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
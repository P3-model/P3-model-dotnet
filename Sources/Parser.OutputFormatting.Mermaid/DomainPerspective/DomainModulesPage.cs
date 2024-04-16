using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using Parser.ModelQuerying;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainModulesPage : MermaidPageBase
{
    private readonly Hierarchy<DomainModule> _modulesHierarchy;

    public override string Header => "Domain Modules";

    protected override string Description => @"This view shows domain model modularization.  
First level modules can be treated as separate sub-models or DDD Bounded Contexts.  
All modules can be divided into sub-modules to reflect hierarchical structure of the domain.";

    public override string RelativeFilePath => Path.Combine("Domain", "Modules", "Modules.md");
    public override ElementBase? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.Domain;

    public DomainModulesPage(string outputDirectory, Hierarchy<DomainModule> modulesHierarchy)
        : base(outputDirectory) =>
        _modulesHierarchy = modulesHierarchy;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Modules hierarchy", 2);
        foreach (var module in _modulesHierarchy.FromLevel(0).OrderBy(m => m.Name))
        {
            mermaidWriter.WriteHeading($"{module.Name}", 2);
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var moduleId = flowchartWriter.WriteRectangle(module.Name, Style.DomainPerspective);
                Write(module, moduleId, flowchartWriter);
            });
        }
    }

    private void Write(DomainModule parent, string parentId, FlowchartElementsWriter flowchartWriter)
    {
        foreach (var child in _modulesHierarchy.GetChildrenFor(parent).OrderBy(r => r.Name))
        {
            var childId = flowchartWriter.WriteRectangle(child.Name, Style.DomainPerspective);
            flowchartWriter.WriteArrow(parentId, childId, "contains");
            Write(child, childId, flowchartWriter);
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is DomainModulePage or
        DomainModuleOwnersPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
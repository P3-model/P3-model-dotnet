using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class ProcessesPage : MermaidPageBase
{
    private readonly IReadOnlySet<Process> _processes;

    public ProcessesPage(string outputDirectory, IReadOnlySet<Process> processes) : base(outputDirectory) =>
        _processes = processes;

    public override string Header => "Business Processes";
    protected override string Description => "This view contains all business processes";
    public override string RelativeFilePath => Path.Combine("Domain", "Processes", "BusinessProcesses.md");
    public override Element? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.Domain;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var process in _processes.OrderBy(p => p.Name))
                flowchartWriter.WriteRectangle(process.Name, Style.DomainPerspective);
        });
        
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is ProcessPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
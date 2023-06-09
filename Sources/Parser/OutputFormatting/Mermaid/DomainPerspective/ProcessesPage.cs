using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

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
    public override string RelativeFilePath => "Business_Processes.md";
    public override Element MainElement => _product;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        foreach (var process in _processesHierarchy.FromFirstLevel())
        {
            mermaidWriter.WriteHeading(process.Name, 2);
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var processId = flowchartWriter.WriteRectangle(process.Name);
                Write(process, processId, flowchartWriter);
            });
        }
    }

    private void Write(Process parent, int parentId, FlowchartElementsWriter flowchartWriter)
    {
        foreach (var child in _processesHierarchy.GetChildrenFor(parent).OrderBy(r => r.Name))
        {
            var childId = flowchartWriter.WriteRectangle(child.Name);
            flowchartWriter.WriteArrow(parentId, childId);
            Write(child, childId, flowchartWriter);
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is ProcessPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}

public class ProcessStepPage : MermaidPageBase
{
    private readonly ProcessStep _step;
    private readonly Process? _process;
    private readonly DomainModule? _domainModule;
    private readonly DeployableUnit? _deployableUnit;
    private readonly IEnumerable<Actor> _actors;
    private readonly IEnumerable<DevelopmentTeam> _developmentTeams;
    private readonly IEnumerable<BusinessOrganizationalUnit> _organizationalUnits;

    public ProcessStepPage(string outputDirectory, ProcessStep step, Process? process, DomainModule? domainModule,
        DeployableUnit? deployableUnit, IEnumerable<Actor> actors, IEnumerable<DevelopmentTeam> developmentTeams,
        IEnumerable<BusinessOrganizationalUnit> organizationalUnits) : base(outputDirectory)
    {
        _step = step;
        _process = process;
        _domainModule = domainModule;
        _deployableUnit = deployableUnit;
        _actors = actors;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }

    public override string Header => $"[*Process Step*] {_step.Name}";
    public override string RelativeFilePath => $"Processes/{_step.Name}.md";
    public override Element MainElement => _step;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var stepId = flowchartWriter.WriteRectangle(_step.Name);
            if (_process != null)
            {
                var processId = flowchartWriter.WriteStadiumShape(_process.Name);
                flowchartWriter.WriteArrow(stepId, processId);
            }

            if (_domainModule != null)
            {
                var moduleId = flowchartWriter.WriteStadiumShape(_domainModule.Name);
                flowchartWriter.WriteArrow(stepId, moduleId);
            }
        });

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var stepId = flowchartWriter.WriteRectangle(_step.Name);
            if (_deployableUnit != null)
            {
                var deployableUnitId = flowchartWriter.WriteStadiumShape(_deployableUnit.Name);
                flowchartWriter.WriteArrow(stepId, deployableUnitId);
            }
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var stepId = flowchartWriter.WriteRectangle(_step.Name);
            foreach (var actor in _actors)
            {
                var actorId = flowchartWriter.WriteStadiumShape(actor.Name);
                flowchartWriter.WriteArrow(actorId, stepId);
            }

            foreach (var team in _developmentTeams)
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name);
                flowchartWriter.WriteArrow(teamId, stepId);
            }

            foreach (var organizationalUnit in _organizationalUnits)
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name);
                flowchartWriter.WriteArrow(organizationalUnitId, stepId);
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page)
    {
        return false;
    }

    protected override bool IncludeInZoomOutPages(MermaidPage page) =>
        page is ProcessPage processPage &&
        processPage.MainElement.Equals(_process);
}
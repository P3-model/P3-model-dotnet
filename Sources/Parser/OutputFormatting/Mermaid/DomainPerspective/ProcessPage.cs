using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class ProcessPage : MermaidPageBase
{
    private readonly Process _process;
    private readonly Process? _parentProcess;
    private readonly IEnumerable<Process> _subProcesses;
    private readonly IEnumerable<ProcessStep> _steps;
    private readonly IEnumerable<DomainModule> _modules;
    private readonly IEnumerable<DeployableUnit> _deployableUnits;
    private readonly IEnumerable<Actor> _actors;
    private readonly IEnumerable<DevelopmentTeam> _developmentTeams;
    private readonly IEnumerable<BusinessOrganizationalUnit> _organizationalUnits;

    public ProcessPage(string outputDirectory, Process process, Process? parentProcess,
        IEnumerable<Process> subProcesses, IEnumerable<ProcessStep> steps, IEnumerable<DomainModule> modules,
        IEnumerable<DeployableUnit> deployableUnits, IEnumerable<Actor> actors,
        IEnumerable<DevelopmentTeam> developmentTeams, IEnumerable<BusinessOrganizationalUnit> organizationalUnits) :
        base(outputDirectory)
    {
        _process = process;
        _parentProcess = parentProcess;
        _subProcesses = subProcesses;
        _steps = steps;
        _modules = modules;
        _deployableUnits = deployableUnits;
        _actors = actors;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }

    public override string Header => $"[*Business process*] {_process.Name}";
    public override string RelativeFilePath => $"Processes/{_process.Name}.md";
    public override Element MainElement => _process;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name);
            if (_parentProcess != null)
            {
                var parentId = flowchartWriter.WriteStadiumShape(_parentProcess.Name);
                flowchartWriter.WriteArrow(parentId, processId);
            }
            foreach (var subProcess in _subProcesses)
            {
                var subProcessId = flowchartWriter.WriteStadiumShape(subProcess.Name);
                flowchartWriter.WriteArrow(processId, subProcessId);
            }
            foreach (var step in _steps)
            {
                var stepId = flowchartWriter.WriteStadiumShape(step.Name);
                flowchartWriter.WriteArrow(processId, stepId);
            }
        });
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name);
            foreach (var module in _modules)
            {
                var moduleId = flowchartWriter.WriteStadiumShape(module.ModulesHierarchy);
                flowchartWriter.WriteArrow(processId, moduleId);
            }
        });

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name);
            foreach (var deployableUnit in _deployableUnits)
            {
                var deployableUnitId = flowchartWriter.WriteStadiumShape(deployableUnit.Name);
                flowchartWriter.WriteArrow(processId, deployableUnitId);
            }
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name);
            foreach (var actor in _actors)
            {
                var actorId = flowchartWriter.WriteStadiumShape(actor.Name);
                flowchartWriter.WriteArrow(actorId, processId);
            }

            foreach (var team in _developmentTeams)
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name);
                flowchartWriter.WriteArrow(teamId, processId);
            }

            foreach (var organizationalUnit in _organizationalUnits)
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name);
                flowchartWriter.WriteArrow(organizationalUnitId, processId);
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) =>
        page is ProcessStepPage stepPage &&
        _steps.Contains(stepPage.MainElement);

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is ProcessesPage;
    
    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;
using P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class ProcessPage : MermaidPageBase
{
    private readonly Process _process;
    private readonly Process? _parent;
    private readonly IEnumerable<Process> _children;
    private readonly IEnumerable<Process.HasNextSubProcess> _processHasNextSubProcessRelations;
    private readonly IEnumerable<ProcessStep> _steps;
    private readonly IEnumerable<DomainModule> _modules;
    private readonly IEnumerable<DeployableUnit> _deployableUnits;
    private readonly IEnumerable<Actor> _actors;
    private readonly IEnumerable<DevelopmentTeam> _developmentTeams;
    private readonly IEnumerable<BusinessOrganizationalUnit> _organizationalUnits;

    public ProcessPage(string outputDirectory, Process process, Process? parent, IEnumerable<Process> children,
        IEnumerable<Process.HasNextSubProcess> processHasNextSubProcessRelations, IEnumerable<ProcessStep> steps,
        IEnumerable<DomainModule> modules, IEnumerable<DeployableUnit> deployableUnits, IEnumerable<Actor> actors,
        IEnumerable<DevelopmentTeam> developmentTeams, IEnumerable<BusinessOrganizationalUnit> organizationalUnits)
        : base(outputDirectory)
    {
        _process = process;
        _parent = parent;
        _children = children;
        _processHasNextSubProcessRelations = processHasNextSubProcessRelations;
        _steps = steps;
        _modules = modules;
        _deployableUnits = deployableUnits;
        _actors = actors;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }

    public override string Header => $"[*Business process*] {_process.Name}";
    protected override string Description => @$"This view contains details information about {_process.Name} business process, including:
- other related processes
- process steps
- related domain modules
- related deployable units
- engaged people: actors, development teams, business stakeholders";

    public override string RelativeFilePath =>
        $"Processes/{string.Join('/', _process.Hierarchy.Parts)}/{_process.Name}.md";

    public override Element MainElement => _process;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related processes and steps", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            if (_parent != null)
            {
                var parentId = flowchartWriter.WriteStadiumShape(_parent.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(parentId, processId);
            }

            var subProcessIds = new Dictionary<Process, int>();
            foreach (var subProcess in _children)
            {
                var subProcessId = flowchartWriter.WriteStadiumShape(subProcess.Name, Style.DomainPerspective);
                subProcessIds.Add(subProcess, subProcessId);
                flowchartWriter.WriteArrow(processId, subProcessId, "contains");
            }

            foreach (var relation in _processHasNextSubProcessRelations)
            {
                var currentProcessId = subProcessIds[relation.Source];
                var nextProcessId = subProcessIds[relation.Destination];
                flowchartWriter.WriteArrow(currentProcessId, nextProcessId, LineStyle.Dotted);
            }

            foreach (var step in _steps)
            {
                var stepId = flowchartWriter.WriteStadiumShape(step.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(processId, stepId, "contains");
            }
        });
        mermaidWriter.WriteHeading("Related modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            foreach (var module in _modules)
            {
                var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(processId, moduleId, "belongs to");
            }
        });

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Related deployable units", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            foreach (var deployableUnit in _deployableUnits)
            {
                var deployableUnitId = flowchartWriter.WriteStadiumShape(deployableUnit.Name, 
                    Style.TechnologyPerspective);
                flowchartWriter.WriteArrow(processId, deployableUnitId, "is deployed in");
            }
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Engaged people", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            foreach (var actor in _actors)
            {
                var actorId = flowchartWriter.WriteStadiumShape(actor.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(actorId, processId, "uses");
            }

            foreach (var team in _developmentTeams)
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(teamId, processId, "develops & maintains");
            }

            foreach (var organizationalUnit in _organizationalUnits)
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name, 
                    Style.PeoplePerspective);
                flowchartWriter.WriteArrow(organizationalUnitId, processId, "owns");
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        ProcessPage processesPage when _children.Contains(processesPage.MainElement) => true,
        ProcessStepPage stepPage when _steps.Contains(stepPage.MainElement) => true,
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is ProcessesPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => page switch
    {
        ProcessPage processPage => _children.Contains(processPage.MainElement),
        ProcessStepPage stepPage => _steps.Contains(stepPage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnits.Contains(deployableUnitPage.MainElement),
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage.MainElement),
        _ => false
    };
}
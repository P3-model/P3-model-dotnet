using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
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

    protected override string Description =>
        @$"This view contains details information about {_process.Name} business process, including:
- other related processes
- process steps
- related domain modules
- related deployable units
- engaged people: actors, development teams, business stakeholders";

    public override string RelativeFilePath => Path.Combine(
        "Domain", "Processes", Path.Combine(_process.Id.Parts.ToArray()), $"{_process.Name.Dehumanize()}.md");

    public override Element? MainElement => _process;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related processes and steps", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            string processId;
            if (_parent is null)
            {
                processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            }
            else
            {
                var parentId = flowchartWriter.WriteStadiumShape(_parent.Name, Style.DomainPerspective);
                processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
                flowchartWriter.WriteBackwardArrow(processId, parentId, "is part of");
            }

            var subProcessIds = new Dictionary<Process, string>();
            foreach (var subProcess in _children.OrderBy(c => c.Name))
            {
                var subProcessId = flowchartWriter.WriteStadiumShape(subProcess.Name, Style.DomainPerspective);
                subProcessIds.Add(subProcess, subProcessId);
                flowchartWriter.WriteArrow(processId, subProcessId, "contains");
            }

            foreach (var relation in _processHasNextSubProcessRelations
                         .OrderBy(r => r.Source.Name)
                         .ThenBy(r => r.Destination.Name))
            {
                var currentProcessId = subProcessIds[relation.Source];
                var nextProcessId = subProcessIds[relation.Destination];
                flowchartWriter.WriteArrow(currentProcessId, nextProcessId, LineStyle.Dotted);
            }

            foreach (var step in _steps.OrderBy(s => s.Name))
            {
                var stepId = flowchartWriter.WriteStadiumShape(step.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(processId, stepId, "contains");
            }
        });
        mermaidWriter.WriteHeading("Related modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            foreach (var module in _modules.OrderBy(m => m.Name))
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
            foreach (var deployableUnit in _deployableUnits.OrderBy(u => u.Name))
            {
                var deployableUnitId = flowchartWriter.WriteStadiumShape(deployableUnit.Name,
                    Style.TechnologyPerspective);
                flowchartWriter.WriteArrow(processId, deployableUnitId, "is deployed in");
            }
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Engaged people", 3);
        mermaidWriter.WriteFlowchart(FlowchartDirection.LR, flowchartWriter =>
        {
            var actorsId = flowchartWriter.WriteSubgraph("Actors", FlowchartDirection.TB, actorsWriter =>
            {
                string? previousActorId = null;
                foreach (var actor in _actors.OrderBy(a => a.Name))
                {
                    var actorId = actorsWriter.WriteStadiumShape(actor.Name, Style.PeoplePerspective);
                    if(previousActorId != null)
                        actorsWriter.WriteInvisibleLink(previousActorId, actorId);
                    previousActorId = actorId;
                }
            });
            var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
            flowchartWriter.WriteArrow(actorsId, processId, "uses");
            var teamsId = flowchartWriter.WriteSubgraph("Teams", FlowchartDirection.TB, teamsWriter =>
            {
                string? previousTeamId = null;
                foreach (var team in _developmentTeams.OrderBy(t => t.Name))
                {
                    var teamId = teamsWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                    if(previousTeamId != null)
                        teamsWriter.WriteInvisibleLink(previousTeamId, teamId);
                    previousTeamId = teamId;
                }
            });
            flowchartWriter.WriteBackwardArrow(teamsId, processId, "develops & maintains");
            var businessId = flowchartWriter.WriteSubgraph("Business", FlowchartDirection.TB, businessWriter =>
            {
                string? previousUnitId = null;
                foreach (var organizationalUnit in _organizationalUnits.OrderBy(u => u.Name))
                {
                    var unitId = businessWriter.WriteStadiumShape(organizationalUnit.Name, Style.PeoplePerspective);
                    if(previousUnitId != null)
                        businessWriter.WriteInvisibleLink(previousUnitId, unitId);
                    previousUnitId = unitId;
                }
            });
            flowchartWriter.WriteBackwardArrow(businessId, processId, "owns");
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        ProcessPage processPage => _children.Contains(processPage.MainElement),
        ProcessStepPage stepPage => _steps.Contains(stepPage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnits.Contains(deployableUnitPage.MainElement),
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is ProcessesPage;
}
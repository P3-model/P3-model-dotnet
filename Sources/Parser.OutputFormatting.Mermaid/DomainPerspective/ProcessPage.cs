using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;
using P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class ProcessPage : MermaidPageBase
{
    private readonly Process _process;
    private readonly IReadOnlySet<ProcessStep> _steps;
    private readonly IReadOnlySet<DomainModule> _modules;
    private readonly IReadOnlySet<DeployableUnit> _deployableUnits;
    private readonly IReadOnlySet<Actor> _actors;
    private readonly IReadOnlySet<DevelopmentTeam> _developmentTeams;
    private readonly IReadOnlySet<BusinessOrganizationalUnit> _organizationalUnits;

    public ProcessPage(string outputDirectory, Process process, IReadOnlySet<ProcessStep> steps,
        IReadOnlySet<DomainModule> modules, IReadOnlySet<DeployableUnit> deployableUnits, 
        IReadOnlySet<Actor> actors, IReadOnlySet<DevelopmentTeam> developmentTeams, 
        IReadOnlySet<BusinessOrganizationalUnit> organizationalUnits) : base(outputDirectory)
    {
        _process = process;
        _steps = steps;
        _modules = modules;
        _deployableUnits = deployableUnits;
        _actors = actors;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }

    protected override string Description =>
        @$"This view contains details information about {_process.Name} business process, including:
- process steps
- related domain modules
- related deployable units
- engaged people: actors, development teams, business stakeholders";

    public override string RelativeFilePath => Path.Combine("Domain", "Processes", $"{_process.Name.Dehumanize()}.md");

    public override ElementBase MainElement => _process;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related process steps", 3);
        if (_steps.Count == 0)
        {
            mermaidWriter.WriteLine("No related process steps were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
                foreach (var step in _steps.OrderBy(s => s.Name))
                {
                    var stepId = flowchartWriter.WriteStadiumShape(step.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(processId, stepId, "contains");
                }
            });
        }

        mermaidWriter.WriteHeading("Related top level domain modules", 3);
        if (_modules.Count == 0)
        {
            mermaidWriter.WriteLine("No related domain modules were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
                foreach (var modelBoundary in _modules.OrderBy(m => m.Name))
                {
                    var modelBoundaryId = flowchartWriter.WriteStadiumShape(modelBoundary.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(processId, modelBoundaryId, "belongs to");
                }
            });
        }

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Related deployable units", 3);
        if (_deployableUnits.Count == 0)
        {
            mermaidWriter.WriteLine("No related deployable units were found.");
        }
        else
        {
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
        }

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Engaged people", 3);
        if (_actors.Count == 0 && _developmentTeams.Count == 0 && _organizationalUnits.Count == 0)
        {
            mermaidWriter.WriteLine("No engaged people were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(FlowchartDirection.LR, flowchartWriter =>
            {
                var actorsId = flowchartWriter.WriteSubgraph("Actors", FlowchartDirection.TB, actorsWriter =>
                {
                    if (_actors.Count == 0)
                    {
                        actorsWriter.WriteStadiumShape("no actors found");
                    }
                    else
                    {
                        string? previousActorId = null;
                        foreach (var actor in _actors.OrderBy(a => a.Name))
                        {
                            var actorId = actorsWriter.WriteStadiumShape(actor.Name, Style.PeoplePerspective);
                            if (previousActorId != null)
                                actorsWriter.WriteInvisibleLink(previousActorId, actorId);
                            previousActorId = actorId;
                        }
                    }
                });
                var processId = flowchartWriter.WriteRectangle(_process.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(actorsId, processId, "uses");
                var teamsId = flowchartWriter.WriteSubgraph("Teams", FlowchartDirection.TB, teamsWriter =>
                {
                    if (_developmentTeams.Count == 0)
                    {
                        teamsWriter.WriteStadiumShape("no teams found");
                    }
                    else
                    {
                        string? previousTeamId = null;
                        foreach (var team in _developmentTeams.OrderBy(t => t.Name))
                        {
                            var teamId = teamsWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                            if (previousTeamId != null)
                                teamsWriter.WriteInvisibleLink(previousTeamId, teamId);
                            previousTeamId = teamId;
                        }
                    }
                });
                flowchartWriter.WriteBackwardArrow(teamsId, processId, "develops & maintains");
                var businessId = flowchartWriter.WriteSubgraph("Business", FlowchartDirection.TB, businessWriter =>
                {
                    if (_organizationalUnits.Count == 0)
                    {
                        businessWriter.WriteStadiumShape("no units found");
                    }
                    else
                    {
                        string? previousUnitId = null;
                        foreach (var organizationalUnit in _organizationalUnits.OrderBy(u => u.Name))
                        {
                            var unitId = businessWriter.WriteStadiumShape(organizationalUnit.Name, Style.PeoplePerspective);
                            if (previousUnitId != null)
                                businessWriter.WriteInvisibleLink(previousUnitId, unitId);
                            previousUnitId = unitId;
                        }
                    }
                });
                flowchartWriter.WriteBackwardArrow(businessId, processId, "owns");
            });
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        ProcessStepPage stepPage => _steps.Contains(stepPage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnits.Contains(deployableUnitPage.MainElement),
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage
            .MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is ProcessesPage;
}
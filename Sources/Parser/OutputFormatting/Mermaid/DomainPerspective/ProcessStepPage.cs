using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

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
    protected override string Description => @$"This view contains details information about {_step.Name} business processes step, including:
- related process
- next process steps
- related domain module
- related deployable unit
- engaged people: actors, development teams, business stakeholders";
    
    public override string RelativeFilePath => _process is null
        ? $"Processes/Steps/{_step.Name}.md"
        : $"Processes/{string.Join('/', _process.Hierarchy.Parts)}/Steps/{_step.Name}.md";
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
                var moduleId = flowchartWriter.WriteCircle(_domainModule.Name);
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
    
    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
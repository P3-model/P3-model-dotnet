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
        ? $"ProcessSteps/{_step.Name}.md"
        : $"ProcessSteps/{string.Join('/', _process.Id.Parts)}/{_step.Name}.md";
    public override Element MainElement => _step;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var stepId = flowchartWriter.WriteRectangle(_step.Name, Style.DomainPerspective);
            if (_process != null)
            {
                var processId = flowchartWriter.WriteStadiumShape(_process.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(stepId, processId, "is part of process");
            }

            if (_domainModule != null)
            {
                var moduleId = flowchartWriter.WriteStadiumShape(_domainModule.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(stepId, moduleId, "belongs to module");
            }
        });

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var stepId = flowchartWriter.WriteRectangle(_step.Name, Style.DomainPerspective);
            if (_deployableUnit != null)
            {
                var deployableUnitId = flowchartWriter.WriteStadiumShape(_deployableUnit.Name, 
                    Style.TechnologyPerspective);
                flowchartWriter.WriteArrow(stepId, deployableUnitId, "is deployed in");
            }
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var stepId = flowchartWriter.WriteRectangle(_step.Name, Style.DomainPerspective);
            foreach (var actor in _actors)
            {
                var actorId = flowchartWriter.WriteStadiumShape(actor.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(actorId, stepId, "uses");
            }

            foreach (var team in _developmentTeams)
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(teamId, stepId, "develops & maintains");
            }

            foreach (var organizationalUnit in _organizationalUnits)
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name, 
                    Style.PeoplePerspective);
                flowchartWriter.WriteArrow(organizationalUnitId, stepId, "owns");
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
    
    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => page switch
    {
        ProcessPage processPage => _process?.Equals(processPage.MainElement) ?? false,
        DomainModulePage modulePage => _domainModule?.Equals(modulePage.MainElement) ?? false,
        DeployableUnitPage deployableUnitPage => _deployableUnit?.Equals(deployableUnitPage.MainElement) ?? false,
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage.MainElement),
        _ => false
    };
}
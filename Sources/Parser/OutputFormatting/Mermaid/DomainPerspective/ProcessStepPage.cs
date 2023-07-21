using System.Collections.Generic;
using System.IO;
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
    private readonly IReadOnlySet<DomainBuildingBlock> _buildingBlocks;
    private readonly DeployableUnit? _deployableUnit;
    private readonly IReadOnlySet<Actor> _actors;
    private readonly IReadOnlySet<DevelopmentTeam> _developmentTeams;
    private readonly IReadOnlySet<BusinessOrganizationalUnit> _organizationalUnits;

    public ProcessStepPage(string outputDirectory, ProcessStep step, Process? process, DomainModule? domainModule,
        IReadOnlySet<DomainBuildingBlock> buildingBlocks, DeployableUnit? deployableUnit, IReadOnlySet<Actor> actors, 
        IReadOnlySet<DevelopmentTeam> developmentTeams, IReadOnlySet<BusinessOrganizationalUnit> organizationalUnits) 
        : base(outputDirectory)
    {
        _step = step;
        _process = process;
        _domainModule = domainModule;
        _buildingBlocks = buildingBlocks;
        _deployableUnit = deployableUnit;
        _actors = actors;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }
    
    protected override string Description => @$"This view contains details information about {_step.Name} business processes step, including:
- related process
- next process steps
- related domain module
- related deployable unit
- engaged people: actors, development teams, business stakeholders";
    
    public override string RelativeFilePath => _process is null
        ? Path.Combine("ProcessSteps", $"{_step.Name}.md")
        : Path.Combine("ProcessSteps", Path.Combine(_process.Id.Parts.ToArray()), $"{_step.Name}.md");
    public override Element? MainElement => _step;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Module & Process", 3);
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
        mermaidWriter.WriteHeading("Used Building Blocks", 3);
        if (_buildingBlocks.Count > 0)
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var stepId = flowchartWriter.WriteRectangle(_step.Name, Style.DomainPerspective);
                foreach (var buildingBlock in _buildingBlocks.OrderBy(b => b.Name))
                {
                    var buildingBlockId = flowchartWriter.WriteStadiumShape(buildingBlock.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(stepId, buildingBlockId, "uses");
                }
            });
        }
        else
        {
            mermaidWriter.WriteLine("No building blocks are used. Maybe this process step is not implemented yet?");
        }

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
            foreach (var actor in _actors.OrderBy(a => a.Name))
            {
                var actorId = flowchartWriter.WriteStadiumShape(actor.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(actorId, stepId, "uses");
            }

            foreach (var team in _developmentTeams.OrderBy(t => t.Name))
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(teamId, stepId, "develops & maintains");
            }

            foreach (var organizationalUnit in _organizationalUnits.OrderBy(u => u.Name))
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name, 
                    Style.PeoplePerspective);
                flowchartWriter.WriteArrow(organizationalUnitId, stepId, "owns");
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        ProcessPage processPage => _process?.Equals(processPage.MainElement) ?? false,
        DomainModulePage modulePage => _domainModule?.Equals(modulePage.MainElement) ?? false,
        DeployableUnitPage deployableUnitPage => _deployableUnit?.Equals(deployableUnitPage.MainElement) ?? false,
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) =>
        page is ProcessPage processPage &&
        processPage.MainElement!.Equals(_process);
}
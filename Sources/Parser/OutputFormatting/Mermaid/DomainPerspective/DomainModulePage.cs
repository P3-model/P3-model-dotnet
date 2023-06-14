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

public class DomainModulePage : MermaidPageBase
{
    private readonly DomainModule _module;
    private readonly DomainModule? _parent;
    private readonly IEnumerable<DomainModule> _children;
    private readonly IEnumerable<Process> _processes;
    private readonly IEnumerable<DeployableUnit> _deployableUnits;
    private readonly IEnumerable<DevelopmentTeam> _developmentTeams;
    private readonly IEnumerable<BusinessOrganizationalUnit> _organizationalUnits;

    public DomainModulePage(string outputDirectory, DomainModule module, DomainModule? parent,
        IEnumerable<DomainModule> children, IEnumerable<Process> processes, IEnumerable<DeployableUnit> deployableUnits, 
        IEnumerable<DevelopmentTeam> developmentTeams, IEnumerable<BusinessOrganizationalUnit> organizationalUnits) 
        : base(outputDirectory)
    {
        _module = module;
        _parent = parent;
        _children = children;
        _processes = processes;
        _deployableUnits = deployableUnits;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }

    public override string Header => $"[*Domain module*] {_module.Name}";

    protected override string Description =>
        @$"This view contains details information about {_module.Name} domain module, including:
- other related modules
- related processes
- related deployable units
- engaged people: actors, development teams, business stakeholders";

    public override string RelativeFilePath =>
        $"Modules/{string.Join('/', _module.Hierarchy.Parts)}/{_module.Name}.md";

    public override Element MainElement => _module;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            if (_parent != null)
            {
                var parentId = flowchartWriter.WriteStadiumShape(_parent.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(moduleId, parentId, "is part of");
            }

            foreach (var child in _children)
            {
                var childId = flowchartWriter.WriteStadiumShape(child.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(moduleId, childId, "contains");
            }
        });
        mermaidWriter.WriteHeading("Related processes", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            foreach (var process in _processes)
            {
                var processId = flowchartWriter.WriteStadiumShape(process.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(moduleId, processId, "contains");
            }
        });

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Related deployable units", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            foreach (var deployableUnit in _deployableUnits)
            {
                var deployableUnitId = flowchartWriter.WriteStadiumShape(deployableUnit.Name,
                    Style.TechnologyPerspective);
                flowchartWriter.WriteArrow(moduleId, deployableUnitId, "is deployed in");
            }
        });

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Engaged people", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            foreach (var team in _developmentTeams)
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(teamId, moduleId, "develops & maintains");
            }
            foreach (var organizationalUnit in _organizationalUnits)
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name,
                    Style.PeoplePerspective);
                flowchartWriter.WriteArrow(organizationalUnitId, moduleId, "owns");
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        ProcessPage processesPage when _children.Contains(processesPage.MainElement) => true,
        ProcessStepPage stepPage when _processes.Contains(stepPage.MainElement) => true,
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is ProcessesPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => page switch
    {
        ProcessPage processPage => _processes.Contains(processPage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnits.Contains(deployableUnitPage.MainElement),
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage.MainElement),
        _ => false
    };
}
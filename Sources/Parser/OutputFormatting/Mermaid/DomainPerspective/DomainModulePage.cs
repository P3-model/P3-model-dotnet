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

public class DomainModulePage : MermaidPageBase
{
    private readonly DomainModule _module;
    private readonly DomainModule? _parent;
    private readonly IReadOnlySet<DomainModule> _children;
    private readonly IReadOnlySet<Process> _processes;
    private readonly IReadOnlySet<DomainBuildingBlock> _directBuildingBlocks;
    private readonly IReadOnlySet<DeployableUnit> _deployableUnits;
    private readonly IReadOnlySet<DevelopmentTeam> _developmentTeams;
    private readonly IReadOnlySet<BusinessOrganizationalUnit> _organizationalUnits;

    public DomainModulePage(string outputDirectory, DomainModule module, DomainModule? parent,
        IReadOnlySet<DomainModule> children, IReadOnlySet<Process> processes, 
        IReadOnlySet<DomainBuildingBlock> directBuildingBlocks, IReadOnlySet<DeployableUnit> deployableUnits, 
        IReadOnlySet<DevelopmentTeam> developmentTeams, IReadOnlySet<BusinessOrganizationalUnit> organizationalUnits) 
        : base(outputDirectory)
    {
        _module = module;
        _parent = parent;
        _children = children;
        _processes = processes;
        _directBuildingBlocks = directBuildingBlocks;
        _deployableUnits = deployableUnits;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
    }

    protected override string Description =>
        @$"This view contains details information about {_module.Name} domain module, including:
- other related modules
- related processes
- related building blocks
- related deployable units
- engaged people: actors, development teams, business stakeholders";

    public override string RelativeFilePath => Path.Combine(
        "Domain", "Concepts", Path.Combine(_module.Id.Parts.ToArray()), $"{_module.Name.Dehumanize()}.md");

    public override Element? MainElement => _module;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related modules", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            string moduleId;
            if (_parent is null)
            {
                moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            }
            else
            {
                var parentId = flowchartWriter.WriteStadiumShape(_parent.Name, Style.DomainPerspective);
                moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
                flowchartWriter.WriteBackwardArrow(moduleId, parentId, "is part of");
            }

            foreach (var child in _children.OrderBy(c => c.Name))
            {
                var childId = flowchartWriter.WriteStadiumShape(child.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(moduleId, childId, "contains");
            }
        });
        mermaidWriter.WriteHeading("Related processes", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            foreach (var process in _processes.OrderBy(p => p.Name))
            {
                var processId = flowchartWriter.WriteStadiumShape(process.Name, Style.DomainPerspective);
                flowchartWriter.WriteArrow(moduleId, processId, "contains");
            }
        });
        mermaidWriter.WriteHeading("Direct building blocks", 3);
        if (_directBuildingBlocks.Count > 0)
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
                foreach (var buildingBlock in _directBuildingBlocks.OrderBy(b => b.Name))
                {
                    var buildingBlockId = flowchartWriter.WriteStadiumShape(buildingBlock.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(moduleId, buildingBlockId, "contains");
                }
            });
        }
        else
        {
            mermaidWriter.WriteLine("Module doesn't contain direct building blocks.");
        }

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Related deployable units", 3);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var moduleId = flowchartWriter.WriteRectangle(_module.Name, Style.DomainPerspective);
            foreach (var deployableUnit in _deployableUnits.OrderBy(u => u.Name))
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
            foreach (var team in _developmentTeams.OrderBy(t => t.Name))
            {
                var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                flowchartWriter.WriteArrow(teamId, moduleId, "develops & maintains");
            }
            foreach (var organizationalUnit in _organizationalUnits.OrderBy(u => u.Name))
            {
                var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name,
                    Style.PeoplePerspective);
                flowchartWriter.WriteArrow(organizationalUnitId, moduleId, "owns");
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => _children.Contains(modulePage.MainElement) ,
        DomainBuildingBlockPage buildingBlockPage => _directBuildingBlocks.Contains(buildingBlockPage.MainElement),
        ProcessPage processPage => _processes.Contains(processPage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnits.Contains(deployableUnitPage.MainElement),
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => _parent is null 
        ? page is DomainModulesPage
        : page is DomainModulePage modulePage && _parent.Equals(modulePage.MainElement);
}
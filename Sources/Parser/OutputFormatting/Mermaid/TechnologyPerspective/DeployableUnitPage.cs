using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;
using P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

public class DeployableUnitPage : MermaidPageBase
{
    private readonly DeployableUnit _unit;
    private readonly Tier? _tier;
    private readonly CSharpProject? _startupProject;
    private readonly HashSet<(CSharpProject Project, IReadOnlySet<Layer> Layers)> _referencedProjects;
    private readonly IReadOnlySet<(Database Database, DatabaseCluster? Cluster)> _databases;
    private readonly IReadOnlySet<DomainModule> _modules;
    private readonly IReadOnlySet<DevelopmentTeam> _teams;

    public DeployableUnitPage(string outputDirectory, DeployableUnit unit, Tier? tier,
        CSharpProject? startupProject, HashSet<(CSharpProject, IReadOnlySet<Layer>)> referencedProjects,
        IReadOnlySet<(Database, DatabaseCluster?)> databases, IReadOnlySet<DomainModule> modules,
        IReadOnlySet<DevelopmentTeam> teams) : base(outputDirectory)
    {
        _unit = unit;
        _tier = tier;
        _startupProject = startupProject;
        _referencedProjects = referencedProjects;
        _databases = databases;
        _modules = modules;
        _teams = teams;
    }

    protected override string Description =>
        @$"This view contains details information about {_unit.Name} deployable unit, including:
- related domain modules
- related development teams";

    public override string RelativeFilePath => Path.Combine("Technology", "DeployableUnits",
        $"{_unit.Name.Dehumanize()}.md");

    public override Element MainElement => _unit;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Related domain modules", 3);
        if (_modules.Count == 0)
        {
            mermaidWriter.WriteLine("No related modules were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
                foreach (var module in _modules.OrderBy(m => m.Name))
                {
                    var moduleId = flowchartWriter.WriteStadiumShape(module.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(moduleId, unitId, "is deployed in");
                }
            });
        }

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        mermaidWriter.WriteHeading("Tier, CSharp Projects and their Layers", 3);
        if (_startupProject is null)
        {
            if (_tier == null)
                mermaidWriter.WriteLine("No related tier and c# projects were found.");
            else
                mermaidWriter.WriteFlowchart(flowchartWriter => flowchartWriter
                    .WriteSubgraph($"{_tier.Name} Tier", subgraphWriter => subgraphWriter
                        .WriteRectangle(_unit.Name, Style.TechnologyPerspective)));
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                if (_tier == null)
                    flowchartWriter.WriteSubgraph(_unit.Name, WriteProjectsAndLayers);
                else
                    flowchartWriter.WriteSubgraph($"{_tier.Name} Tier", tierSubgraphWriter => tierSubgraphWriter
                        .WriteSubgraph(_unit.Name, WriteProjectsAndLayers));
            });
        }
        mermaidWriter.WriteHeading("Infrastructure", 3);
        if (_databases.Count == 0)
        {
            mermaidWriter.WriteLine("No infrastructure elements were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
                foreach (var group in _databases
                             .GroupBy(item => item.Cluster, item => item.Database)
                             .OrderBy(item => item.Key?.Name))
                {
                    var cluster = group.Key;
                    if (cluster is null)
                    {
                        foreach (var database in group.OrderBy(d => d.Name))
                        {
                            var databaseId = flowchartWriter.WriteStadiumShape(database.Name,
                                Style.TechnologyPerspective);
                            flowchartWriter.WriteArrow(unitId, databaseId, "uses");
                        }
                    }
                    else
                    {
                        var clusterId = flowchartWriter.WriteSubgraph(cluster.Name, subgraphWriter =>
                        {
                            foreach (var database in group.OrderBy(d => d.Name))
                                subgraphWriter.WriteStadiumShape(database.Name, Style.TechnologyPerspective);
                        });
                        flowchartWriter.WriteArrow(unitId, clusterId, "uses");
                    }
                }
            });
        }

        mermaidWriter.WriteHeading("People Perspective", 2);
        mermaidWriter.WriteHeading("Related development teams", 3);
        if (_teams.Count == 0)
        {
            mermaidWriter.WriteLine("No related development teams were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var unitId = flowchartWriter.WriteRectangle(_unit.Name, Style.TechnologyPerspective);
                foreach (var team in _teams.OrderBy(t => t.Name))
                {
                    var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                    flowchartWriter.WriteArrow(teamId, unitId, "maintains");
                }
            });
        }
    }

    private void WriteProjectsAndLayers(FlowchartElementsWriter writer)
    {
        var undefinedLayerProjects = new HashSet<CSharpProject>();
        var layersToProjects = new Dictionary<Layer, HashSet<CSharpProject>>();
        foreach (var (project, layers) in _referencedProjects)
        {
            if (layers.Count == 0)
            {
                undefinedLayerProjects.Add(project);
            }
            else
            {
                foreach (var layer in layers)
                {
                    if (!layersToProjects.TryGetValue(layer, out var projects))
                        layersToProjects.Add(layer, projects = new HashSet<CSharpProject>());
                    projects.Add(project);
                }
            }
        }
        if (undefinedLayerProjects.Count > 0)
            layersToProjects.Add(new Layer("Undefined"), undefinedLayerProjects);
        foreach (var (layer, projects) in layersToProjects.OrderBy(pair => pair.Key.Name))
        {
            writer.WriteSubgraph($"{layer.Name} Layer", layerSubgraphWriter =>
            {
                foreach (var project in projects.OrderBy(p => p.Name))
                    layerSubgraphWriter.WriteStadiumShape(project.Name, Style.TechnologyPerspective);
            });
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => _modules.Contains(modulePage.MainElement),
        DevelopmentTeamPage teamPage => _teams.Contains(teamPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is DeployableUnitsPage;
}
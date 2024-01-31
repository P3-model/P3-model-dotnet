using System.Collections.Generic;
using System.IO;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainModuleOwnersPage : MermaidPageBase
{
    private readonly HashSet<DomainModule> _modules;
    private readonly Dictionary<DevelopmentTeam, HashSet<DomainModule>> _teamToModules;
    private readonly Dictionary<BusinessOrganizationalUnit, HashSet<DomainModule>> _unitToModules;
    private readonly Dictionary<BusinessOrganizationalUnit, HashSet<DevelopmentTeam>> _unitToTeams;

    public override string Header => "Domain Module Owners";

    protected override string Description => @"This view shows how top level domain modules are assigned to development teams and business organizational units.";

    public override string RelativeFilePath => Path.Combine("Domain", "Modules", "ModuleOwners.md");
    public override ElementBase? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.Domain;

    public DomainModuleOwnersPage(string outputDirectory, IEnumerable<DomainModuleOwners> modulesOwners) 
        : base(outputDirectory)
    {
        _modules = new HashSet<DomainModule>();
        _teamToModules = new Dictionary<DevelopmentTeam, HashSet<DomainModule>>();
        _unitToModules = new Dictionary<BusinessOrganizationalUnit, HashSet<DomainModule>>();
        _unitToTeams = new Dictionary<BusinessOrganizationalUnit, HashSet<DevelopmentTeam>>();
        foreach (var moduleOwners in modulesOwners)
        {
            _modules.Add(moduleOwners.Module);
            foreach (var team in moduleOwners.DevelopmentOwners) 
                _teamToModules.AddToValues(team, moduleOwners.Module);
            foreach (var unit in moduleOwners.BusinessOwners)
            {
                _unitToModules.AddToValues(unit, moduleOwners.Module);
                foreach (var team in moduleOwners.DevelopmentOwners) 
                    _unitToTeams.AddToValues(unit, team);
            }
        }
    }

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Development Teams and top level Domain Modules they maintain", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var (team, modules) in _teamToModules)
            {
                flowchartWriter.WriteSubgraph(team.Name, subgraphWriter =>
                {
                    foreach (var module in modules) 
                        subgraphWriter.WriteRectangle(module.Name, Style.DomainPerspective);
                });
            }
        });
        
        mermaidWriter.WriteHeading("Business Organizational Units and top level Domain Modules they own", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var (unit, modules) in _unitToModules)
            {
                flowchartWriter.WriteSubgraph(unit.Name, subgraphWriter =>
                {
                    foreach (var module in modules)
                        subgraphWriter.WriteRectangle(module.Name, Style.DomainPerspective);
                });
            }
        });
        
        mermaidWriter.WriteHeading("Business Organizational Units and related Development Teams", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            foreach (var (unit, teams) in _unitToTeams)
            {
                flowchartWriter.WriteSubgraph(unit.Name, subgraphWriter =>
                {
                    foreach (var team in teams)
                        subgraphWriter.WriteRectangle(team.Name, Style.PeoplePerspective);
                });
            }
        });
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => _modules.Contains((DomainModule)modulePage.MainElement),
        DevelopmentTeamPage teamPage => _teamToModules.ContainsKey((DevelopmentTeam)teamPage.MainElement),
        BusinessOrganizationalUnitPage unitPage => 
            _unitToModules.ContainsKey((BusinessOrganizationalUnit)unitPage.MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is 
        MainPage or
        DomainModulesPage or
        DevelopmentTeamsPage or
        BusinessOrganizationalUnitsPage;
}
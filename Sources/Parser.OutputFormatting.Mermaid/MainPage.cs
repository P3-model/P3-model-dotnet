using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.OutputFormatting.Mermaid.Domain;
using P3Model.Parser.OutputFormatting.Mermaid.People;
using P3Model.Parser.OutputFormatting.Mermaid.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MainPage : MermaidPageBase
{
    private readonly DocumentedSystem _system;
    private readonly IReadOnlySet<Actor> _actors;
    private readonly IReadOnlySet<ExternalSystem> _externalSystems;
    private readonly IReadOnlySet<DevelopmentTeam> _developmentTeams;
    private readonly IReadOnlySet<BusinessOrganizationalUnit> _organizationalUnits;
    private readonly DomainVisionStatement? _domainVisionStatement;

    public MainPage(string outputDirectory, 
        DocumentedSystem system, 
        IReadOnlySet<Actor> actors, 
        IReadOnlySet<ExternalSystem> externalSystems, 
        IReadOnlySet<DevelopmentTeam> developmentTeams, 
        IReadOnlySet<BusinessOrganizationalUnit> organizationalUnits, 
        DomainVisionStatement? domainVisionStatement) 
        : base(outputDirectory)
    {
        _system = system;
        _actors = actors;
        _externalSystems = externalSystems;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
        _domainVisionStatement = domainVisionStatement;
    }

    public override string Header => $"P3 Model documentation for {_system.Name}";
    public override string LinkText => "Main page";
    public override string RelativeFilePath => "README.md";
    public override ElementBase? MainElement => null;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        WriteSystemLandscape(mermaidWriter);
        WriteDomainVisionStatement(mermaidWriter);
    }

    private void WriteSystemLandscape(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("System Landscape", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var systemName = _system.Name.EndsWith("system", StringComparison.InvariantCultureIgnoreCase)
                ? _system.Name
                : $"{_system.Name} system";
            var systemId = flowchartWriter.WriteRectangle(systemName);
            var actorsId = flowchartWriter.WriteSubgraph("Actors", subgraphWriter =>
            {
                if (_actors.Count == 0)
                {
                    subgraphWriter.WriteStadiumShape("no actors found");
                }
                else
                {
                    foreach (var actor in _actors.OrderBy(a => a.Name)) 
                        subgraphWriter.WriteStadiumShape(actor.Name);
                }
            });
            flowchartWriter.WriteArrow(actorsId, systemId, "uses");
            var externalSystemsId = flowchartWriter.WriteSubgraph("External Systems", subgraphWriter =>
            {
                if (_externalSystems.Count == 0)
                {
                    subgraphWriter.WriteStadiumShape("no external systems found");
                }
                else
                {
                    foreach (var externalSystem in _externalSystems.OrderBy(s => s.Name)) 
                        subgraphWriter.WriteStadiumShape(externalSystem.Name);
                }
            });
            flowchartWriter.WriteBidirectionalArrow(externalSystemsId, systemId, "are integrated with");
            var teamsId = flowchartWriter.WriteSubgraph("Development Teams", subgraphWriter =>
            {
                if (_developmentTeams.Count == 0)
                {
                    subgraphWriter.WriteStadiumShape("no teams found");
                }
                else
                {
                    foreach (var team in _developmentTeams.OrderBy(t => t.Name)) 
                        subgraphWriter.WriteStadiumShape(team.Name);
                }
            });
            flowchartWriter.WriteBackwardArrow(teamsId, systemId, "develops & maintains");
            var businessUnitsId = flowchartWriter.WriteSubgraph("Business Units", subgraphWriter =>
            {
                if (_organizationalUnits.Count == 0)
                {
                    subgraphWriter.WriteStadiumShape("no units found");
                }
                else
                {
                    foreach (var unit in _organizationalUnits.OrderBy(u => u.Name)) 
                        subgraphWriter.WriteStadiumShape(unit.Name);
                }
            });
            flowchartWriter.WriteBackwardArrow(businessUnitsId, systemId, "owns");
        });
    }

    private void WriteDomainVisionStatement(MermaidWriter mermaidWriter)
    {
        if (_domainVisionStatement is null)
            return;
        var sourceFileInfo = _domainVisionStatement.SourceFile;
        var relativeFilePath = sourceFileInfo.Name;
        var fileInfo = sourceFileInfo.CopyTo(GetAbsolutePath(relativeFilePath));
        var pathRelativeToPageFile = GetPathRelativeToPageFile(fileInfo.FullName);
        mermaidWriter.WriteHeading("Domain Vision Statement", 2);
        mermaidWriter.WriteLinkInline("Link", pathRelativeToPageFile);
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => page is
        DomainGlossaryPage or
        DomainModulesPage or
        DomainModuleOwnersPage or
        ProcessesPage or
        DevelopmentTeamsPage or
        BusinessOrganizationalUnitsPage or
        DeployableUnitsPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => false;
}
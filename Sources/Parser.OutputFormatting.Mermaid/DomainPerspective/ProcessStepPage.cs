using Humanizer;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;
using P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class UseCasePage : MermaidPageBase
{
    private readonly UseCase _useCase;
    private readonly DomainModule? _module;
    private readonly Process? _process;
    private readonly IReadOnlySet<DomainBuildingBlock> _buildingBlocks;
    private readonly DeployableUnit? _deployableUnit;
    private readonly IReadOnlySet<Actor> _actors;
    private readonly IReadOnlySet<DevelopmentTeam> _developmentTeams;
    private readonly IReadOnlySet<BusinessOrganizationalUnit> _organizationalUnits;
    private readonly IReadOnlySet<CodeStructure> _codeStructures;

    public UseCasePage(string outputDirectory, UseCase useCase, DomainModule? module, Process? process,
        IReadOnlySet<DomainBuildingBlock> buildingBlocks, DeployableUnit? deployableUnit, IReadOnlySet<Actor> actors,
        IReadOnlySet<DevelopmentTeam> developmentTeams, IReadOnlySet<BusinessOrganizationalUnit> organizationalUnits, 
        IReadOnlySet<CodeStructure> codeStructures)
        : base(outputDirectory)
    {
        _useCase = useCase;
        _module = module;
        _process = process;
        _buildingBlocks = buildingBlocks;
        _deployableUnit = deployableUnit;
        _actors = actors;
        _developmentTeams = developmentTeams;
        _organizationalUnits = organizationalUnits;
        _codeStructures = codeStructures;
    }

    protected override string Description =>
        @$"This view contains details information about {_useCase.Name} use case, including:
- related process
- related domain module
- related deployable unit
- engaged people: actors, development teams, business stakeholders";

    public override string RelativeFilePath => _module is null
        ? Path.Combine("Domain", "Modules", $"{_useCase.Name.Dehumanize()}.md")
        : Path.Combine("Domain", "Modules", Path.Combine(_module.HierarchyPath.Parts.ToArray()),
            $"{_useCase.Name.Dehumanize()}.md");
    
    public override ElementBase MainElement => _useCase;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Perspective", 2);
        mermaidWriter.WriteHeading("Process", 3);
        if (_process is null)
        {
            mermaidWriter.WriteLine("No related process was found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var useCaseId = flowchartWriter.WriteRectangle(_useCase.Name, Style.DomainPerspective);
                {
                    var processId = flowchartWriter.WriteStadiumShape(_process.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(useCaseId, processId, "is part of");
                }
            });
        }

        mermaidWriter.WriteHeading("Used Building Blocks", 3);
        if (_buildingBlocks.Count == 0)
        {
            mermaidWriter.WriteLine("No building blocks were found. Maybe this use case is not implemented yet?");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var useCaseId = flowchartWriter.WriteRectangle(_useCase.Name, Style.DomainPerspective);
                foreach (var buildingBlock in _buildingBlocks.OrderBy(b => b.Name))
                {
                    var buildingBlockId =
                        flowchartWriter.WriteStadiumShape(buildingBlock.Name, Style.DomainPerspective);
                    flowchartWriter.WriteArrow(useCaseId, buildingBlockId, "uses");
                }
            });
        }

        mermaidWriter.WriteHeading("Technology Perspective", 2);
        if (_deployableUnit is null)
        {
            mermaidWriter.WriteLine("No related deployable unit was found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var useCaseId = flowchartWriter.WriteRectangle(_useCase.Name, Style.DomainPerspective);
                var deployableUnitId = flowchartWriter.WriteStadiumShape(_deployableUnit.Name,
                    Style.TechnologyPerspective);
                flowchartWriter.WriteArrow(useCaseId, deployableUnitId, "is deployed in");
            });
        }
        mermaidWriter.WriteHeading("Source code", 3);
        if (_codeStructures.Count == 1)
            mermaidWriter.WriteLine(FormatSourceCodeLink(_codeStructures.First()));
        else
            mermaidWriter.WriteUnorderedList(_codeStructures.OrderBy(s => s.SourceCodeSourceCodePath), FormatSourceCodeLink);

        mermaidWriter.WriteHeading("People Perspective", 2);
        if (_actors.Count == 0 && _developmentTeams.Count == 0 && _organizationalUnits.Count == 0)
        {
            mermaidWriter.WriteLine("No engaged people were found.");
        }
        else
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                var useCaseId = flowchartWriter.WriteRectangle(_useCase.Name, Style.DomainPerspective);
                foreach (var actor in _actors.OrderBy(a => a.Name))
                {
                    var actorId = flowchartWriter.WriteStadiumShape(actor.Name, Style.PeoplePerspective);
                    flowchartWriter.WriteArrow(actorId, useCaseId, "uses");
                }

                foreach (var team in _developmentTeams.OrderBy(t => t.Name))
                {
                    var teamId = flowchartWriter.WriteStadiumShape(team.Name, Style.PeoplePerspective);
                    flowchartWriter.WriteArrow(teamId, useCaseId, "develops & maintains");
                }

                foreach (var organizationalUnit in _organizationalUnits.OrderBy(u => u.Name))
                {
                    var organizationalUnitId = flowchartWriter.WriteStadiumShape(organizationalUnit.Name,
                        Style.PeoplePerspective);
                    flowchartWriter.WriteArrow(organizationalUnitId, useCaseId, "owns");
                }
            });
        }
    }

    private string FormatSourceCodeLink(CodeStructure codeStructure) => MermaidWriter
        .FormatLink(Path.GetFileName(codeStructure.SourceCodeSourceCodePath), GetPathRelativeToPageFile(codeStructure.SourceCodeSourceCodePath));
    
    protected override bool IncludeInZoomInPages(MermaidPage page) => page switch
    {
        DomainBuildingBlockPage buildingBlockPage => _buildingBlocks.Contains(buildingBlockPage.MainElement),
        DeployableUnitPage deployableUnitPage => _deployableUnit?.Equals(deployableUnitPage.MainElement) ?? false,
        DevelopmentTeamPage developmentTeamPage => _developmentTeams.Contains(developmentTeamPage.MainElement),
        BusinessOrganizationalUnitPage organizationalUnitPage => _organizationalUnits.Contains(organizationalUnitPage
            .MainElement),
        _ => false
    };

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page switch
    {
        DomainModulePage modulePage => modulePage.MainElement.Equals(_module),
        ProcessPage processPage => processPage.MainElement.Equals(_process),
        _ => false
    };
}
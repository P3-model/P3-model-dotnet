using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;
using P3Model.Parser.OutputFormatting.Mermaid.PeoplePerspective;
using P3Model.Parser.OutputFormatting.Mermaid.TechnologyPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MainPage : MermaidPageBase
{
    private readonly ModelSyntax.DocumentedSystem _system;
    private readonly IReadOnlySet<Actor> _actors;
    private readonly IReadOnlySet<ExternalSoftwareSystem> _inputExternalSystems;
    private readonly IReadOnlySet<ExternalSoftwareSystem> _outputExternalSystems;
    private readonly DomainVisionStatement? _domainVisionStatement;

    public MainPage(string outputDirectory, 
        ModelSyntax.DocumentedSystem system, 
        IReadOnlySet<Actor> actors, 
        IReadOnlySet<ExternalSoftwareSystem> inputExternalSystems, 
        IReadOnlySet<ExternalSoftwareSystem> outputExternalSystems, 
        DomainVisionStatement? domainVisionStatement) 
        : base(outputDirectory)
    {
        _system = system;
        _actors = actors;
        _inputExternalSystems = inputExternalSystems;
        _outputExternalSystems = outputExternalSystems;
        _domainVisionStatement = domainVisionStatement;
    }

    public override string Header => $"P3 Model documentation for {_system.Name}";
    public override string LinkText => "Main page";
    public override string RelativeFilePath => "README.md";
    public override Element? MainElement => null;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        WriteProductLandscape(mermaidWriter);
        WriteDomainVisionStatement(mermaidWriter);
    }

    private void WriteProductLandscape(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Product Landscape", 2);
        mermaidWriter.WriteFlowchart(flowchartWriter =>
        {
            var productId = flowchartWriter.WriteRectangle(_system.Name);
            foreach (var actor in _actors)
            {
                var actorId = flowchartWriter.WriteCircle(actor.Name);
                flowchartWriter.WriteArrow(actorId, productId);
            }

            var externalSystemIds = new Dictionary<ExternalSoftwareSystem, string>();
            foreach (var externalSystem in _outputExternalSystems)
            {
                if (!externalSystemIds.TryGetValue(externalSystem, out var externalSystemId))
                {
                    externalSystemId = flowchartWriter.WriteRectangle(externalSystem.Name);
                    externalSystemIds.Add(externalSystem, externalSystemId);
                }

                flowchartWriter.WriteArrow(productId, externalSystemId);
            }

            foreach (var externalSystem in _inputExternalSystems)
            {
                if (!externalSystemIds.TryGetValue(externalSystem, out var externalSystemId))
                {
                    externalSystemId = flowchartWriter.WriteRectangle(externalSystem.Name);
                    externalSystemIds.Add(externalSystem, externalSystemId);
                }

                flowchartWriter.WriteArrow(externalSystemId, productId);
            }
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
        ProcessesPage or
        DevelopmentTeamsPage or
        BusinessOrganizationalUnitsPage or
        DeployableUnitsPage;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => false;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
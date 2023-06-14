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
    private readonly Product _product;
    private readonly IReadOnlyCollection<Actor.UsesProduct> _actorUsesProductRelations;
    private readonly IReadOnlyCollection<Product.UsesExternalSystem> _productUsesExternalSystemRelations;
    private readonly IReadOnlyCollection<ExternalSystem.UsesProduct> _externalSystemUsesProductRelations;
    private readonly DomainVisionStatement? _domainVisionStatement;

    public MainPage(string outputDirectory, Product product,
        IReadOnlyCollection<Actor.UsesProduct> actorUsesProductRelations,
        IReadOnlyCollection<Product.UsesExternalSystem> productUsesExternalSystemRelations,
        IReadOnlyCollection<ExternalSystem.UsesProduct> externalSystemUsesProductRelations,
        DomainVisionStatement? domainVisionStatement) : base(outputDirectory)
    {
        _product = product;
        _actorUsesProductRelations = actorUsesProductRelations;
        _productUsesExternalSystemRelations = productUsesExternalSystemRelations;
        _externalSystemUsesProductRelations = externalSystemUsesProductRelations;
        _domainVisionStatement = domainVisionStatement;
    }

    public override string Header => $"P3 Model documentation for {_product.Name}";
    public override string LinkText => "Main page";
    public override string RelativeFilePath => "README.md";
    public override Element MainElement => _product;

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
            var productId = flowchartWriter.WriteRectangle(_product.Name);
            foreach (var actorUsesProduct in _actorUsesProductRelations)
            {
                var actorId = flowchartWriter.WriteCircle(actorUsesProduct.Actor.Name);
                flowchartWriter.WriteArrow(actorId, productId);
            }

            var externalSystemIds = new Dictionary<ExternalSystem, int>();
            foreach (var productUsesExternalSystem in _productUsesExternalSystemRelations)
            {
                if (!externalSystemIds.TryGetValue(productUsesExternalSystem.ExternalSystem, out var externalSystemId))
                {
                    externalSystemId = flowchartWriter.WriteRectangle(productUsesExternalSystem.ExternalSystem.Name);
                    externalSystemIds.Add(productUsesExternalSystem.ExternalSystem, externalSystemId);
                }

                flowchartWriter.WriteArrow(productId, externalSystemId);
            }

            foreach (var externalSystemUsesProduct in _externalSystemUsesProductRelations)
            {
                if (!externalSystemIds.TryGetValue(externalSystemUsesProduct.ExternalSystem, out var externalSystemId))
                {
                    externalSystemId = flowchartWriter.WriteRectangle(externalSystemUsesProduct.ExternalSystem.Name);
                    externalSystemIds.Add(externalSystemUsesProduct.ExternalSystem, externalSystemId);
                }

                flowchartWriter.WriteArrow(externalSystemId, productId);
            }
        });
    }

    private void WriteDomainVisionStatement(MermaidWriter mermaidWriter)
    {
        if (_domainVisionStatement is null)
            return;
        mermaidWriter.WriteHeading("Domain Vision Statement", 2);
        mermaidWriter.WriteLinkInline("Link", _domainVisionStatement.FileInfo.FullName);
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
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainGlossaryPage : MermaidPageBase
{
    private readonly Product _product;
    private readonly DomainModulesHierarchy _modulesHierarchy;
    private readonly DomainBuildingBlocks _buildingBlocks;

    public override string Header => "Domain Glossary";
    protected override string Description => "This view contains definitions of key domain terms.";
    public override string RelativeFilePath => "Domain_Glossary.md";
    public override Element MainElement => _product;

    public DomainGlossaryPage(string outputDirectory, Product product, DomainModulesHierarchy modulesHierarchy, 
        DomainBuildingBlocks buildingBlocks) : base(outputDirectory)
    {
        _product = product;
        _modulesHierarchy = modulesHierarchy;
        _buildingBlocks = buildingBlocks;
    }

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Glossary", 1);
        foreach (var module in _modulesHierarchy.FromLevel(0).OrderBy(m => m.Name))
            Write(module, mermaidWriter, 2);
    }

    private void Write(DomainModule parentModule, MermaidWriter mermaidWriter, int level)
    {
        mermaidWriter.WriteHeading(parentModule.Name, level);

        foreach (var childModule in _modulesHierarchy.GetChildrenFor(parentModule).OrderBy(r => r.Name))
            Write(childModule, mermaidWriter, level + 1);

        foreach (var buildingBlock in _buildingBlocks.GetFor(parentModule).OrderBy(r => r.Name))
        {
            mermaidWriter.WriteInline($"**{buildingBlock.Name}**");
            WriteDescriptionLink(mermaidWriter, buildingBlock);
            mermaidWriter.WriteLineBreak();
            mermaidWriter.WriteLineBreak();
        }
    }

    private static void WriteDescriptionLink(MermaidWriter mermaidWriter, DomainBuildingBlock buildingBlock)
    {
        if (buildingBlock.DescriptionFile is null)
            return;
        mermaidWriter.WriteInline(" - ");
        mermaidWriter.WriteLinkInline("Long description", buildingBlock.DescriptionFile.FullName);
        mermaidWriter.WriteLineBreak();
    }
    
    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
    
    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
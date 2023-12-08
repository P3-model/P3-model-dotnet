using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainGlossaryPage : MermaidPageBase
{
    private readonly Hierarchy<DomainModule> _modulesHierarchy;
    private readonly IReadOnlyCollection<BuildingBlockInfo> _buildingBlocks;

    public override string Header => "Domain Glossary";
    protected override string Description => "This view contains definitions of key domain terms.";
    public override string RelativeFilePath => Path.Combine("Domain", "Glossary", "Domain_Glossary.md");
    public override ElementBase? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.Domain;

    public DomainGlossaryPage(string outputDirectory, Hierarchy<DomainModule> modulesHierarchy,
        IReadOnlyCollection<BuildingBlockInfo> buildingBlocks)
        : base(outputDirectory)
    {
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

        foreach (var buildingBlockInfo in _buildingBlocks
                     .Where(r => r.Module.Equals(parentModule))
                     .OrderBy(r => r.BuildingBlock.Name))
        {
            mermaidWriter.WriteInline($"**{buildingBlockInfo.BuildingBlock.Name}**");
            WriteDescriptionLink(mermaidWriter, buildingBlockInfo.BuildingBlock, buildingBlockInfo.DescriptionTrait);
            mermaidWriter.WriteLineBreak();
            mermaidWriter.WriteLineBreak();
        }
    }

    private void WriteDescriptionLink(MermaidWriter mermaidWriter, DomainBuildingBlock buildingBlock, 
        DomainBuildingBlockDescription? descriptionTrait)
    {
        if (descriptionTrait is null)
            return;
        var sourceFileInfo = descriptionTrait.DescriptionFile;
        var relativeFilePath = Path.Combine("Domain", "Glossary", sourceFileInfo.Name);
        var fileInfo = sourceFileInfo.CopyTo(GetAbsolutePath(relativeFilePath));
        var pathRelativeToPageFile = GetPathRelativeToPageFile(fileInfo.FullName);
        mermaidWriter.WriteInline(" - ");
        mermaidWriter.WriteLinkInline("Long description", pathRelativeToPageFile);
        mermaidWriter.WriteLineBreak();
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    public record BuildingBlockInfo(DomainBuildingBlock BuildingBlock, 
        DomainModule Module,
        DomainBuildingBlockDescription? DescriptionTrait);
}
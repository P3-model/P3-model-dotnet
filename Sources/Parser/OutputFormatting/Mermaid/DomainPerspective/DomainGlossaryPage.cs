using System.Collections.Generic;
using System.IO;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainGlossaryPage : MermaidPageBase
{
    private readonly Hierarchy<DomainModule> _modulesHierarchy;
    private readonly IReadOnlySet<DomainModule.ContainsBuildingBlock> _containsBuildingBlockRelations;

    public override string Header => "Domain Glossary";
    protected override string Description => "This view contains definitions of key domain terms.";
    public override string RelativeFilePath => Path.Combine("Glossary", "Domain_Glossary.md");
    public override Element? MainElement => null;

    public DomainGlossaryPage(string outputDirectory, Hierarchy<DomainModule> modulesHierarchy,
        IReadOnlySet<DomainModule.ContainsBuildingBlock> containsBuildingBlockRelations)
        : base(outputDirectory)
    {
        _modulesHierarchy = modulesHierarchy;
        _containsBuildingBlockRelations = containsBuildingBlockRelations;
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

        foreach (var buildingBlock in _containsBuildingBlockRelations
                     .Where(r => r.Source.Equals(parentModule))
                     .Select(r => r.Destination)
                     .OrderBy(r => r.Name))
        {
            mermaidWriter.WriteInline($"**{buildingBlock.Name}**");
            WriteDescriptionLink(mermaidWriter, buildingBlock);
            mermaidWriter.WriteLineBreak();
            mermaidWriter.WriteLineBreak();
        }
    }

    private void WriteDescriptionLink(MermaidWriter mermaidWriter, DomainBuildingBlock buildingBlock)
    {
        if (buildingBlock.DescriptionFile is null)
            return;
        var sourceFileInfo = buildingBlock.DescriptionFile;
        var relativeFilePath = Path.Combine("Glossary", sourceFileInfo.Name);
        var fileInfo = sourceFileInfo.CopyTo(GetAbsolutePath(relativeFilePath));
        var pathRelativeToPageFile = GetPathRelativeToPageFile(fileInfo.FullName);
        mermaidWriter.WriteInline(" - ");
        mermaidWriter.WriteLinkInline("Long description", pathRelativeToPageFile);
        mermaidWriter.WriteLineBreak();
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;

    protected override bool IncludeInChangePerspectivePages(MermaidPage page) => false;
}
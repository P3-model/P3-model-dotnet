using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.OutputFormatting.Mermaid.Domain;

public class DomainGlossaryPage(
    string outputDirectory,
    Hierarchy<DomainModule> modulesHierarchy,
    IReadOnlyDictionary<DomainModule, IReadOnlyCollection<DomainBuildingBlock>> buildingBlocks)
    : MermaidPageBase(outputDirectory)
{
    public override string Header => "Domain Glossary";
    protected override string Description => "This view contains definitions of key domain terms.";
    public override string RelativeFilePath => Path.Combine("Domain", "Glossary.md");
    public override ElementBase? MainElement => null;
    public override Perspective? Perspective => ModelSyntax.Perspective.Domain;

    protected override void WriteBody(MermaidWriter mermaidWriter)
    {
        mermaidWriter.WriteHeading("Domain Glossary", 1);
        foreach (var module in modulesHierarchy.FromLevel(0).OrderBy(m => m.Name))
            Write(module, mermaidWriter, 2);
    }

    private void Write(DomainModule parentModule, MermaidWriter mermaidWriter, int level)
    {
        mermaidWriter.WriteHeading(parentModule.Name, level);

        foreach (var childModule in modulesHierarchy.GetChildrenFor(parentModule).OrderBy(r => r.Name))
            Write(childModule, mermaidWriter, level + 1);

        if (!buildingBlocks.TryGetValue(parentModule, out var moduleBuildingBlocks))
            return;
        foreach (var buildingBlock in moduleBuildingBlocks.OrderBy(r => r.Name))
        {
            mermaidWriter.WriteInline($"**{buildingBlock.Name}**");
            mermaidWriter.WriteLineBreak();
            mermaidWriter.WriteLineBreak();
        }
    }

    protected override bool IncludeInZoomInPages(MermaidPage page) => false;

    protected override bool IncludeInZoomOutPages(MermaidPage page) => page is MainPage;
}
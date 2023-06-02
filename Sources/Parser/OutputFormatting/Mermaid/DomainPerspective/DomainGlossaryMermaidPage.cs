using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class DomainGlossaryMermaidPage : MermaidPage
{
    public string RelativeFilePath => "Domain_Glossary.md";

    public void Write(Model model, MermaidWriter mermaidWriter, MermaidPages allPages)
    {
        mermaidWriter.WriteHeading("Domain Glossary", 1);
        foreach (var module in model.Elements
                     .OfType<DomainModule>()
                     .Where(m => m.Level == 0)
                     .OrderBy(m => m.Name)) 
            Write(module, model, mermaidWriter, 2);
    }

    private static void Write(DomainModule parentModule, Model model, MermaidWriter mermaidWriter, int level)
    {
        mermaidWriter.WriteHeading(parentModule.Name, level);
        
        foreach (var childModule in model.Relations
                     .OfType<DomainModule.ContainsDomainModule>()
                     .Where(r => r.Parent == parentModule)
                     .OrderBy(r => r.Child.Name)
                     .Select(r => r.Child))
        {
            Write(childModule, model, mermaidWriter, level + 1);
        }
        foreach (var buildingBlock in model.Relations
                     .OfType<ModuleContainsBuildingBlock>()
                     .Where(r => r.DomainModule == parentModule)
                     .OrderBy(r => r.BuildingBlock.Name)
                     .Select(r => r.BuildingBlock))
        {
            mermaidWriter.WriteText($"**{buildingBlock.Name}**");
            WriteDescriptionLink(mermaidWriter, buildingBlock);
            mermaidWriter.WriteEmptyLine();
            mermaidWriter.WriteEmptyLine();
        }
    }

    private static void WriteDescriptionLink(MermaidWriter mermaidWriter, BuildingBlock buildingBlock)
    {
        if (buildingBlock.DescriptionFile is null)
            return;
        mermaidWriter.WriteText(" - ");
        mermaidWriter.WriteLink("Long description", buildingBlock.DescriptionFile.FullName);
        mermaidWriter.WriteEmptyLine();
    }
}
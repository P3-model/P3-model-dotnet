using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public class ModelBoundaryMermaidPage : MermaidPage
{
    public string RelativeFilePath => "Model_Boundaries.md";

    public void Write(Model model, MermaidWriter mermaidWriter, MermaidPages allPages)
    {
        mermaidWriter.WriteHeading("Model boundaries", 1);
        if (model.Elements.OfType<ModelBoundary>().Any())
        {
            mermaidWriter.WriteFlowchart(flowchartWriter =>
            {
                foreach (var modelBoundary in model.Elements
                             .OfType<ModelBoundary>()
                             .OrderBy(m => m.Name))
                    flowchartWriter.WriteRectangle(modelBoundary.Name);
            });
        }
        mermaidWriter.WriteHeading("Next steps", 2);
        if (allPages.TryGetFileRelativePathFor<DomainModulesMermaidPage>(out var path))
            mermaidWriter.WriteLink("Modules", path);
    }
}
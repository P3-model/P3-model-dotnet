using System.Collections.Generic;
using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid;

public class MainPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new MainPage(outputDirectory,
            model.Elements.OfType<Product>().Single(),
            model.Relations.OfType<Actor.UsesProduct>().ToList().AsReadOnly(),
            model.Relations.OfType<Product.UsesExternalSystem>().ToList().AsReadOnly(),
            model.Relations.OfType<ExternalSystem.UsesProduct>().ToList().AsReadOnly());
    }
}
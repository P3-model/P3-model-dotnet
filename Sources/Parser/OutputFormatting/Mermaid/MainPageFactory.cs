using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[UsedImplicitly]
public class MainPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, Model model)
    {
        yield return new MainPage(outputDirectory,
            model.Elements.OfType<Product>().Single(),
            model.Relations.OfType<Actor.UsesProduct>().ToList().AsReadOnly(),
            model.Relations.OfType<Product.UsesExternalSystem>().ToList().AsReadOnly(),
            model.Relations.OfType<ExternalSystem.UsesProduct>().ToList().AsReadOnly(),
            model.Traits.OfType<DomainVisionStatement>().SingleOrDefault());
    }
}
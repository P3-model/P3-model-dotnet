using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[UsedImplicitly]
public class MainPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new MainPage(outputDirectory,
            modelGraph.Execute(query => query
                .SingleElement<Product>())!,
            modelGraph.Execute(query => query
                .Relations<Actor.UsesProduct>()),
            modelGraph.Execute(query => query
                .Relations<Product.UsesExternalSystem>()),
            modelGraph.Execute(query => query
                .Relations<ExternalSystem.UsesProduct>()),
            modelGraph.Execute(query => query
                    .Traits<DomainVisionStatement>())
                .SingleOrDefault());
    }
}
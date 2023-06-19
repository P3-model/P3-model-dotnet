using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelQuerying.Queries;
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
            modelGraph.Execute(Query
                .Elements<Product>()
                .Single()),
            modelGraph.Execute(Query
                .Relations<Actor.UsesProduct>()),
            modelGraph.Execute(Query
                .Relations<Product.UsesExternalSystem>()),
            modelGraph.Execute(Query
                .Relations<ExternalSystem.UsesProduct>()),
            modelGraph.Execute(Query
                    .Traits<DomainVisionStatement>())
                .SingleOrDefault());
    }
}
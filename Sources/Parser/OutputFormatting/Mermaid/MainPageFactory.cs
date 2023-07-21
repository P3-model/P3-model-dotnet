using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[UsedImplicitly]
public class MainPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        yield return new MainPage(outputDirectory,
            modelGraph.System,
            modelGraph.Execute(query => query.AllElements<Actor>()),
            modelGraph.Execute(query => query.AllElements<ExternalSoftwareSystem>()),
            modelGraph.Execute(query => query.AllElements<DevelopmentTeam>()),
            modelGraph.Execute(query => query.AllElements<BusinessOrganizationalUnit>()),
            // TODO: new concept for DomainVisionStatement
            null);
    }
}
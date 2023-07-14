using System.Collections.Generic;
using JetBrains.Annotations;
using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid;

[UsedImplicitly]
public class MainPageFactory : MermaidPageFactory
{
    public IEnumerable<MermaidPage> Create(string outputDirectory, ModelGraph modelGraph)
    {
        var steps = modelGraph.Execute(query => query
            .AllElements<ProcessStep>());
        yield return new MainPage(outputDirectory,
            modelGraph.System,
            modelGraph.Execute(query => query
                .Elements<Actor>()
                .RelatedToAny(steps)
                .ByRelation<Actor.UsesProcessStep>()),
            // TODO: Implement when relations are ready.
            new HashSet<ExternalSoftwareSystem>(),
            new HashSet<ExternalSoftwareSystem>(),
            // TODO: new concept for DomainVisionStatement
            null);
    }
}
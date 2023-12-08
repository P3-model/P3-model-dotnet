using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetStaticElements<TElement> : ElementsQuery<TElement>
    where TElement : class, Element
{
    private readonly IReadOnlySet<TElement> _elements;

    public GetStaticElements(IReadOnlySet<TElement> elements) => _elements = elements;

    public IReadOnlySet<TElement> ExecuteFor(ModelGraph modelGraph) => _elements;
}
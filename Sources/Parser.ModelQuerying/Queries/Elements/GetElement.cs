using P3Model.Parser.ModelSyntax;

namespace Parser.ModelQuerying.Queries.Elements;

public class GetElement<TElement> : ElementQuery<TElement>
    where TElement : class, Element
{
    private readonly Func<TElement, bool>? _predicate;
    
    public GetElement(Func<TElement, bool>? predicate) => _predicate = predicate;

    public TElement? ExecuteFor(ModelGraph modelGraph) => _predicate is null
        ? modelGraph.ElementsOfType<TElement>().SingleOrDefault()
        : modelGraph.ElementsOfType<TElement>().SingleOrDefault(_predicate);
}
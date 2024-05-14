using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public class GetElements<TElement> : ElementsQuery<TElement>
    where TElement : class, Element
{
    private readonly Func<TElement, bool>? _predicate;

    public GetElements(Func<TElement, bool>? predicate) => _predicate = predicate;

    public IReadOnlySet<TElement> ExecuteFor(ModelGraph modelGraph) => _predicate is null
        ? modelGraph.ElementsOfType<TElement>().ToHashSet()
        : modelGraph.ElementsOfType<TElement>().Where(e => _predicate(e)).ToHashSet();
}
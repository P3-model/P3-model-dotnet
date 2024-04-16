using P3Model.Parser.ModelSyntax;

namespace Parser.ModelQuerying.Queries.Elements;

public interface ElementsQuery<TElement>
    where TElement : class, Element
{
    IReadOnlySet<TElement> ExecuteFor(ModelGraph modelGraph);
}
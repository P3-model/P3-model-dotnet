using P3Model.Parser.ModelSyntax;

namespace Parser.ModelQuerying.Queries.Elements;

public interface ElementQuery<out TElement>
    where TElement : class, Element
{
    TElement? ExecuteFor(ModelGraph modelGraph);
}
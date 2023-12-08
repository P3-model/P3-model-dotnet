using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Elements;

public interface ElementsQuery<TElement>
    where TElement : class, Element
{
    IReadOnlySet<TElement> ExecuteFor(ModelGraph modelGraph);
}
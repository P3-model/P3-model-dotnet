using System.Collections.Generic;
using P3Model.Parser.ModelQuerying.Queries.Elements;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Hierarchies;

public class GetDescendants<TElement, TRelation> : ElementsQuery<TElement>
    where TElement : class, HierarchyElement 
    where TRelation : HierarchyRelation<TElement>
{
    private readonly TElement _element;
    private readonly bool _includeSelf;

    public GetDescendants(TElement element, bool includeSelf)
    {
        _element = element;
        _includeSelf = includeSelf;
    }

    public IReadOnlySet<TElement> ExecuteFor(ModelGraph modelGraph)
    {
        var hierarchy = modelGraph.HierarchyFor<TElement, TRelation>();
        return hierarchy.GetDescendantsFor(_element, _includeSelf);
    }
}
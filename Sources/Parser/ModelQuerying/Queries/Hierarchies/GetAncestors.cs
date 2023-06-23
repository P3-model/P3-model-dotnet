using System.Collections.Generic;
using P3Model.Parser.ModelQuerying.Queries.Elements;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Hierarchies;

public class GetAncestors<TElement, TRelation> : ElementsQuery<TElement>
    where TElement : HierarchyElement 
    where TRelation : HierarchyRelation<TElement>
{
    private readonly TElement _element;
    
    public GetAncestors(TElement element) => _element = element;

    public IReadOnlySet<TElement> ExecuteFor(ModelGraph modelGraph)
    {
        var hierarchy = modelGraph.HierarchyFor<TElement, TRelation>();
        return hierarchy.GetAncestorsFor(_element);
    }
}
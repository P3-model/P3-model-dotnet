using System.Diagnostics.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Hierarchies;

[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Marker argument for ModelGraph methods")]
public class GetHierarchy<TElement, TRelation> : HierarchyQuery<TElement>
    where TElement : class, HierarchyElement
    where TRelation : HierarchyRelation<TElement>
{
    public Hierarchy<TElement> Execute(ModelGraph modelGraph) => modelGraph.HierarchyFor<TElement, TRelation>();
}
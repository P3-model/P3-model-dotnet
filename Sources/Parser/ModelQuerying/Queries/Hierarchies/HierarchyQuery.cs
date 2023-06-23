using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Hierarchies;

public interface HierarchyQuery<TElement>
    where TElement : HierarchyElement
{
    Hierarchy<TElement> Execute(ModelGraph modelGraph);
}
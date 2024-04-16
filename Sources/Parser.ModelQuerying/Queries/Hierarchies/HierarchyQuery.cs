using P3Model.Parser.ModelSyntax;

namespace Parser.ModelQuerying.Queries.Hierarchies;

public interface HierarchyQuery<TElement>
    where TElement : class, HierarchyElement
{
    Hierarchy<TElement> Execute(ModelGraph modelGraph);
}
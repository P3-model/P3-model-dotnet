namespace P3Model.Parser.ModelSyntax;

public interface HierarchyRelation<out TElement> : Relation<TElement, TElement> where TElement : Element { }
namespace P3Model.Parser.ModelSyntax;

public abstract class HierarchyRelation<TElement>(TElement source, TElement destination) 
    : RelationBase<TElement, TElement>(source, destination)
    where TElement : class, Element;
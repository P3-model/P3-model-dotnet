namespace P3Model.Parser.ModelSyntax;

public interface Relation { }

public interface RelationFrom<out TSource> : Relation
    where TSource : Element
{
    TSource Source { get; }
}

public interface RelationTo<out TDestination> : Relation
    where TDestination : Element
{
    TDestination Destination { get; }
}

public interface Relation<out TSource, out TDestination> : RelationFrom<TSource>, RelationTo<TDestination>
    where TSource : Element
    where TDestination : Element { }
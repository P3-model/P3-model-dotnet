namespace P3Model.Parser.ModelSyntax;

public interface Relation
{
    Element Source { get; }
    Element Destination { get; }
}

public interface RelationFrom<out TSource> : Relation
    where TSource : class, Element
{
    Element Relation.Source => Source;

    new TSource Source { get; }
}

public interface RelationTo<out TDestination> : Relation
    where TDestination : class, Element
{
    Element Relation.Destination => Destination;
    new TDestination Destination { get; }
}

public interface Relation<out TSource, out TDestination> : RelationFrom<TSource>, RelationTo<TDestination>
    where TSource : class, Element
    where TDestination : class, Element { }
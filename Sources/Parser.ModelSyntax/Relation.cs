using DotNetExtensions;

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
    where TDestination : class, Element;

public abstract class RelationBase<TSource, TDestination>(TSource source, TDestination destination)
    : Relation<TSource, TDestination>, IEquatable<Relation<TSource, TDestination>>
    where TSource : class, Element
    where TDestination : class, Element
{
    public TSource Source { get; } = source;
    public TDestination Destination { get; } = destination;

    public override bool Equals(object? obj) => obj is Relation<TSource, TDestination> other && Equals(other);

    public bool Equals(Relation<TSource, TDestination>? other) =>
        other != null &&
        Source.Equals(other.Source) &&
        Destination.Equals(other.Destination);

    public override int GetHashCode() => HashCode.Combine(Source, Destination);

    public override string ToString() => $"{GetType().GetFullTypeName()}: {Source.Id} -> {Destination.Id}";
}
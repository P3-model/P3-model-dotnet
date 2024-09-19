using System.Diagnostics.CodeAnalysis;

namespace P3Model.Parser.ModelSyntax.DotNetExtensions;

public static class LinqExtensions
{
    public static IEnumerable<T> Apply<T>(
        this IEnumerable<T> enumerable, Func<IEnumerable<T>, IEnumerable<T>> filter) =>
        filter(enumerable);

    public static T? Apply<T>(this IEnumerable<T> enumerable, Func<IEnumerable<T>, T?> filter) =>
        filter(enumerable);

    [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
    public static IEnumerable<TResult> SelectIfNotNull<TSource, TResult>(this IEnumerable<TSource> items,
        Func<TSource, TResult?> selector)
    {
        foreach (var item in items)
        {
            var result = selector(item);
            if (result != null)
                yield return result;
        }
    }

    public static IEnumerable<T> SelectRecursively<T>(this IEnumerable<T> items,
        Func<T, IEnumerable<T>> childrenSelector)
    {
        var stack = new Stack<T>(items);
        while (stack.Any())
        {
            var next = stack.Pop();
            yield return next;
            foreach (var child in childrenSelector(next))
                stack.Push(child);
        }
    }
}
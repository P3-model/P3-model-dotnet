using System;
using System.Collections.Generic;
using System.Linq;

namespace P3Model.Parser;

public static class LinqExtensions
{
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
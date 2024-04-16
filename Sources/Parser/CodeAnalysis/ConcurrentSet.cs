using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace P3Model.Parser.CodeAnalysis;

public class ConcurrentSet<T> : IReadOnlySet<T>
    where T : notnull
{
    private readonly ConcurrentDictionary<T, byte> _items;

    public ConcurrentSet() => _items = new ConcurrentDictionary<T, byte>();

    public ConcurrentSet(IEqualityComparer<T> comparer) => _items = new ConcurrentDictionary<T, byte>(comparer);

    public int Count => _items.Count;

    [PublicAPI]
    public bool TryAdd(T item) => _items.TryAdd(item, default);

    [PublicAPI]
    public bool TryRemove(T item) => _items.TryRemove(item, out _);

    public bool Contains(T item) => _items.ContainsKey(item);

    public bool IsProperSubsetOf(IEnumerable<T> other) => throw new NotSupportedException();

    public bool IsProperSupersetOf(IEnumerable<T> other) => throw new NotSupportedException();

    public bool IsSubsetOf(IEnumerable<T> other) => throw new NotSupportedException();

    public bool IsSupersetOf(IEnumerable<T> other) => throw new NotSupportedException();

    public bool Overlaps(IEnumerable<T> other) => throw new NotSupportedException();

    public bool SetEquals(IEnumerable<T> other) => throw new NotSupportedException();

    public IEnumerator<T> GetEnumerator() => _items.Keys.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
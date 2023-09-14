using System.Collections.Generic;

namespace P3Model.Parser;

public static class DictionaryExtensions
{
    public static void AddToValues<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dictionary,
        TKey key, TValue value)
        where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var set))
            dictionary.Add(key, set = new HashSet<TValue>());
        set.Add(value);
    }
}
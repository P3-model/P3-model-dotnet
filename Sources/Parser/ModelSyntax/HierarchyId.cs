using System.Linq;
using JetBrains.Annotations;

namespace P3Model.Parser.ModelSyntax;

public readonly struct HierarchyId
{
    [PublicAPI] public const char DefaultSeparator = '.';

    private readonly char _separator;
    private readonly string _value;

    public HierarchyId(params string[] parts) : this(string.Join(DefaultSeparator, parts)) { }

    public HierarchyId(char separator, params string[] parts) : this(string.Join(separator, parts), separator) { }

    public HierarchyId(string value, char separator = DefaultSeparator)
    {
        _separator = separator;
        _value = value;
    }

    public string Last => _value[(_value.LastIndexOf(_separator) + 1)..];
    public string Full => _value;
    
    public int Level => _value.Count(c => c == '.');
}
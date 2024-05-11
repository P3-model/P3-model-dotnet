using JetBrains.Annotations;

namespace P3Model.Parser.ModelSyntax;

public readonly struct HierarchyPath : IEquatable<HierarchyPath>
{
    [PublicAPI]
    public const char DefaultSeparator = '.';

    private readonly char _separator;
    private readonly string _value;

    [PublicAPI]
    public static HierarchyPath FromParts(params string[] parts) => FromParts(DefaultSeparator, parts);

    [PublicAPI]
    public static HierarchyPath FromParts(IEnumerable<string> parts) => FromParts(DefaultSeparator, parts);

    [PublicAPI]
    public static HierarchyPath FromParts(char separator, params string[] parts) =>
        FromParts(separator, (IEnumerable<string>)parts);

    [PublicAPI]
    public static HierarchyPath FromParts(char separator, IEnumerable<string> parts) =>
        new(string.Join(separator, parts), separator);

    [PublicAPI]
    public static HierarchyPath FromValue(string value, char separator = DefaultSeparator) => new(value, separator);

    private HierarchyPath(string value, char separator = DefaultSeparator)
    {
        _separator = separator;
        _value = value;
    }

    [PublicAPI]
    public string Full => _value;

    [PublicAPI]
    public string LastPart => _value[(_value.LastIndexOf(_separator) + 1)..];

    [PublicAPI]
    public string Parent => _value.Contains(_separator)
        ? _value[.._value.LastIndexOf(_separator)]
        : string.Empty;

    [PublicAPI]
    public string Root => _value.Contains(_separator)
        ? _value[.._value.IndexOf(_separator)]
        : _value;

    [PublicAPI]
    public int Level => _value.Count(c => c == '.');

    [PublicAPI]
    public IEnumerable<string> Parts => _value.Split(_separator);

    [PublicAPI]
    public bool IsAncestorOf(HierarchyPath other) => other._value.Length < _value.Length &&
        other._value.StartsWith(_value);

    [PublicAPI]
    public bool IsDescendantOf(HierarchyPath other) => other.Level < Level && _value.StartsWith(other._value);

    public override bool Equals(object? obj) => obj is HierarchyPath other && Equals(other);
    public bool Equals(HierarchyPath other) => _value == other._value;
    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(HierarchyPath left, HierarchyPath right) => left.Equals(right);

    public static bool operator !=(HierarchyPath left, HierarchyPath right) => !(left == right);

    public override string ToString() => _value;
}
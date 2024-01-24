using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ExternalSystemAttribute(string? name = null) : Attribute
{
    public string? Name { get; } = name;
}
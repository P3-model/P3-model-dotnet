using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ExternalSoftwareSystemAttribute : Attribute
{
    public string? Name { get; }

    public ExternalSoftwareSystemAttribute(string? name = null) => Name = name;
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly)]
public class ExternalSoftwareSystemAttribute : Attribute
{
    public string? Name { get; }

    public ExternalSoftwareSystemAttribute(string? name = null) => Name = name;
}
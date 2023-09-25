using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class DatabaseAttribute : Attribute
{
    public string Name { get; }
    public string? ClusterName { get; init; }

    public DatabaseAttribute(string name) => Name = name;
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Struct |
                AttributeTargets.Enum |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class DomainBuildingBlockAttribute : Attribute
{
    public string? Name { get; }

    public DomainBuildingBlockAttribute(string? name = null) => Name = name;
}
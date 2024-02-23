using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Struct |
                AttributeTargets.Enum |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class DomainBuildingBlockAttribute(string? name = null) : Attribute, DomainPerspectiveAttribute
{
    public string? Name { get; } = name;
}
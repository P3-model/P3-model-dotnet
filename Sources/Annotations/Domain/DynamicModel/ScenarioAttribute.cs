using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Struct |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Method |
                AttributeTargets.Field |
                AttributeTargets.Property)]
public class ScenarioAttribute(string? name = null) : Attribute, DomainPerspectiveAttribute
{
    public string? Name { get; } = name;
}
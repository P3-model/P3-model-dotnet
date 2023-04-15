using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Struct |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Method |
                AttributeTargets.Field |
                AttributeTargets.Property)]
public class ScenarioAttribute : Attribute
{
    public string? Name { get; }

    public ScenarioAttribute(string? name = null) => Name = name;
}
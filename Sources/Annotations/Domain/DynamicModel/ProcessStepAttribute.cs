using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class ProcessStepAttribute : Attribute
{
    public string? Name { get; }

    public ProcessStepAttribute(string? name = null) => Name = name;
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method |
                AttributeTargets.Delegate |
                AttributeTargets.Interface)]
public class BehaviorAttribute(string? name = null) : Attribute, DomainPerspectiveAttribute
{
    public string? Name { get; } = name;
}
using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method |
                AttributeTargets.Delegate |
                AttributeTargets.Interface)]
public class BehaviorAttribute : Attribute
{
    public string? Name { get; }

    public BehaviorAttribute(string? name = null) => Name = name;
}
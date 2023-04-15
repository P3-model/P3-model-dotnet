using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method |
                AttributeTargets.Delegate |
                AttributeTargets.Interface)]
public class BusinessRuleAttribute : Attribute
{
    public string? Name { get; }

    public BusinessRuleAttribute(string? name = null) => Name = name;
}
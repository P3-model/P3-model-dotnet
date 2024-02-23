using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Struct |
                AttributeTargets.Enum |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class ExternalSystemIntegrationAttribute(string externalSystemName, string? name = null)
    : Attribute, DomainPerspectiveAttribute
{
    public string ExternalSystemName { get; } = externalSystemName;
    public string? Name { get; } = name;
}
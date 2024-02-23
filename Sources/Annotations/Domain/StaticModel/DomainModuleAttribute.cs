using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly |
                AttributeTargets.Class |
                AttributeTargets.Struct |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Enum |
                AttributeTargets.Module)]
public class DomainModuleAttribute(string? name = null) : Attribute, NamespaceApplicable, DomainPerspectiveAttribute
{
    public string? Name { get; } = name;
    public bool ApplyOnNamespace { get; init;  }
}
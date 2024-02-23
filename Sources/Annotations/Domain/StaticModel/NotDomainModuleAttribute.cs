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
public class NotDomainModuleAttribute : Attribute, NamespaceApplicable, DomainPerspectiveAttribute
{
    public bool ApplyOnNamespace { get; init; } = true;
}
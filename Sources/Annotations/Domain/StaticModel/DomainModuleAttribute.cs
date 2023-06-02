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
public class DomainModuleAttribute : Attribute, NamespaceApplicable
{
    public string? Name { get; }
    public bool ApplyOnNamespace { get; }

    public DomainModuleAttribute(string? name = null, bool applyOnNamespace = false)
    {
        Name = name;
        ApplyOnNamespace = applyOnNamespace;
    }
    
    public DomainModuleAttribute(bool applyOnNamespace) => ApplyOnNamespace = applyOnNamespace;
}
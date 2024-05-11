using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
public class SkipNamespaceInDomainModulesHierarchyAttribute : Attribute, 
    NamespaceApplicable, 
    DomainPerspectiveAttribute
{
    public bool ApplyOnNamespace { get; init; }
}
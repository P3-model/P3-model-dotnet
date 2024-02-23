using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public class DomainModelAttribute : Attribute, NamespaceApplicable, DomainPerspectiveAttribute
{
    public bool ApplyOnNamespace { get; init; }
}
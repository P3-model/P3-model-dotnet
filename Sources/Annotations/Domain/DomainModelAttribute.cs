using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public class DomainModelAttribute : Attribute, NamespaceApplicable
{
    public bool ApplyOnNamespace { get; }

    public DomainModelAttribute(bool applyOnNamespace = false) => ApplyOnNamespace = applyOnNamespace;

}
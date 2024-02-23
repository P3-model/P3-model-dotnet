using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ProcessAttribute(string name) : Attribute, NamespaceApplicable, DomainPerspectiveAttribute
{
    public string Name { get; } = name;
    public bool ApplyOnNamespace { get; init; }
}
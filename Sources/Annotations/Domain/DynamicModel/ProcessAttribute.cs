using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ProcessAttribute : Attribute, NamespaceApplicable
{
    public string? Name { get; }
    public bool ApplyOnNamespace { get; }
    public string? Parent { get; init; }

    public ProcessAttribute(string? name = null, bool applyOnNamespace = false)
    {
        Name = name;
        ApplyOnNamespace = applyOnNamespace;
    }

    public ProcessAttribute(bool applyOnNamespace) => ApplyOnNamespace = applyOnNamespace;
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ProcessAttribute : Attribute, NamespaceApplicable
{
    public string Name { get; }
    public bool ApplyOnNamespace { get; init; }

    public ProcessAttribute(string name) => Name = name;
}
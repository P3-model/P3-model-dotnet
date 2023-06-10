using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ProcessAttribute : Attribute, NamespaceApplicable
{
    public string[] FullName { get; }
    public bool ApplyOnNamespace { get; init; }
    public string[]? NextSubProcesses { get; init; }

    public ProcessAttribute(params string[] fullName) => FullName = fullName;
}
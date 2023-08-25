using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ProcessStepContractAttribute : Attribute
{
    public string? Name { get; }

    public ProcessStepContractAttribute(string? name = null) => Name = name;
}
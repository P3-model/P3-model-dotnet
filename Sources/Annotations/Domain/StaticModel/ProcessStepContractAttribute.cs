using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ProcessStepContractAttribute(string? name = null) : Attribute, DomainPerspectiveAttribute
{
    public string? Name { get; } = name;
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Enum)]
public class ExternalContractAttribute(string modelBoundary) : Attribute, DomainPerspectiveAttribute
{
    public string ModelBoundary { get; } = modelBoundary;
}
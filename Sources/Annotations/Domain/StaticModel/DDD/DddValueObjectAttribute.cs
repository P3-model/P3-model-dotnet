using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddValueObjectAttribute : DomainBuildingBlockAttribute
{
    public DddValueObjectAttribute(string? name = null) : base(name) { }
}
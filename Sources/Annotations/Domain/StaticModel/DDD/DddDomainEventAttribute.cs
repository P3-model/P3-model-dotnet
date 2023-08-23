using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddDomainEventAttribute : DomainBuildingBlockAttribute
{
    public DddDomainEventAttribute(string? name = null) : base(name) { }
}
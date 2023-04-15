using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddDomainEventAttribute : ModelBuildingBlockAttribute
{
    public DddDomainEventAttribute(string? name = null) : base(name) { }
}
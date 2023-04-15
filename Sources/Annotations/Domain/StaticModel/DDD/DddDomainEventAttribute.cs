using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddDomainEventAttribute : ModelBuildingBlockAttribute
{
    public DddDomainEventAttribute(string? name = null) : base(name) { }
}
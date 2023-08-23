using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddDomainServiceAttribute : DomainBuildingBlockAttribute
{
    public DddDomainServiceAttribute(string? name = null) : base(name) { }
}
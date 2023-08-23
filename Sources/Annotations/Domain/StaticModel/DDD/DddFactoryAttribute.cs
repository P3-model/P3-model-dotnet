using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddFactoryAttribute : DomainBuildingBlockAttribute
{
    public DddFactoryAttribute(string? name = null) : base(name) { }
}
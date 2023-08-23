using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddRepositoryAttribute : DomainBuildingBlockAttribute
{
    public DddRepositoryAttribute(string? name = null) : base(name) { }
}
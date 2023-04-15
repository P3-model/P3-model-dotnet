using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddRepositoryAttribute : ModelBuildingBlockAttribute
{
    public DddRepositoryAttribute(string? name = null) : base(name) { }
}
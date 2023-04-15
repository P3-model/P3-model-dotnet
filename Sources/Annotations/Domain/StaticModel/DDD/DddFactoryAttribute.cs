using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddFactoryAttribute : ModelBuildingBlockAttribute
{
    public DddFactoryAttribute(string? name = null) : base(name) { }
}
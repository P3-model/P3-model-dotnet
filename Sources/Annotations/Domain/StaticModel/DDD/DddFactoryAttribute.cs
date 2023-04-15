using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddFactoryAttribute : ModelBuildingBlockAttribute
{
    public DddFactoryAttribute(string? name = null) : base(name) { }
}
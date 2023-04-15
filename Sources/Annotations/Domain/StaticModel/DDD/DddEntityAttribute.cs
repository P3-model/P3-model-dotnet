using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddEntityAttribute : ModelBuildingBlockAttribute
{
    public DddEntityAttribute(string? name = null) : base(name) { }
}
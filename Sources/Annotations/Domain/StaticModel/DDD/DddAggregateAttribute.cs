using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddAggregateAttribute : ModelBuildingBlockAttribute
{
    public DddAggregateAttribute(string? name = null) : base(name) { }
}
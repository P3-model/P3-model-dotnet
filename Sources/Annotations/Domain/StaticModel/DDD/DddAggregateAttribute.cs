using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddAggregateAttribute : ModelBuildingBlockAttribute
{
    public DddAggregateAttribute(string? name = null) : base(name) { }
}
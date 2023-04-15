using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddValueObjectAttribute : ModelBuildingBlockAttribute
{
    public DddValueObjectAttribute(string? name = null) : base(name) { }
}
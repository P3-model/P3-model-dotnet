using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddBoundedContextAttribute : ModelBoundaryAttribute
{
    public DddBoundedContextAttribute(string? name = null) : base(name) { }
}
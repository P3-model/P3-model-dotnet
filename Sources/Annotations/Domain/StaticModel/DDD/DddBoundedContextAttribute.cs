using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddBoundedContextAttribute : DomainModuleAttribute
{
    public DddBoundedContextAttribute(string? name = null) : base(name) { }
}
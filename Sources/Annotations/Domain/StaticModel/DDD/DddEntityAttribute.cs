using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddEntityAttribute : DomainBuildingBlockAttribute
{
    public DddEntityAttribute(string? name = null) : base(name) { }
}
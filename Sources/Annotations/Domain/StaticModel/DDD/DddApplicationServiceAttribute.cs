using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddApplicationServiceAttribute : ProcessStepAttribute
{
    public DddApplicationServiceAttribute(string? name = null) : base(name) { }
}
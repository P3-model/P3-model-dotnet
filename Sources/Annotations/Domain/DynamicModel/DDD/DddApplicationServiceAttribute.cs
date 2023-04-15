using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.DynamicModel.DDD;

[PublicAPI]
public class DddApplicationServiceAttribute : ProcessStepAttribute
{
    public DddApplicationServiceAttribute(string? name = null) : base(name) { }
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddApplicationServiceAttribute(string? name = null) : UseCaseAttribute(name);
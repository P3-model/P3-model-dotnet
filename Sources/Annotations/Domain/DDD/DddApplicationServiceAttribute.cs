using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

[PublicAPI]
public class DddApplicationServiceAttribute(string? name = null) : UseCaseAttribute(name);
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

[PublicAPI]
public class DddEntityAttribute(string? name = null) : DomainBuildingBlockAttribute(name);
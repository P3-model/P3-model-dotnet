using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

[PublicAPI]
public class DddFactoryAttribute(string? name = null) : DomainBuildingBlockAttribute(name);
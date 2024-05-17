using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

[PublicAPI]
public class DddValueObjectAttribute(string? name = null) : DomainBuildingBlockAttribute(name);
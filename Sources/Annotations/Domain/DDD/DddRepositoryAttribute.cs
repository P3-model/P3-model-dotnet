using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

[PublicAPI]
public class DddRepositoryAttribute(string? name = null)
    : DomainBuildingBlockAttribute(name), DomainPerspectiveAttribute;
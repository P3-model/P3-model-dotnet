using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddFactoryAttribute(string? name = null) : DomainBuildingBlockAttribute(name), DomainPerspectiveAttribute;
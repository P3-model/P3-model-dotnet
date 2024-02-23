using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddEntityAttribute(string? name = null) : DomainBuildingBlockAttribute(name), DomainPerspectiveAttribute;
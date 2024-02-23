using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

[PublicAPI]
public class DddBoundedContextAttribute(string? name = null) : DomainModuleAttribute(name), DomainPerspectiveAttribute;
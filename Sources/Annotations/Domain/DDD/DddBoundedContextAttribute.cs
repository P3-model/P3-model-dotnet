using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

[PublicAPI]
public class DddBoundedContextAttribute(string? name = null) : DomainModuleAttribute(name), DomainPerspectiveAttribute;
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class QueryAttribute : Attribute, DomainPerspectiveAttribute;
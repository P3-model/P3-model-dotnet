using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
[AttributeUsage(AttributeTargets.All)]
public class ExcludeFromDomainGlossaryAttribute : Attribute { }
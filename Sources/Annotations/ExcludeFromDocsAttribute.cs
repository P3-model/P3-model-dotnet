using JetBrains.Annotations;

namespace P3Model.Annotations;

[PublicAPI]
[AttributeUsage(AttributeTargets.All)]
public class ExcludeFromDocsAttribute : Attribute { }
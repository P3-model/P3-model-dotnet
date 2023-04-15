using JetBrains.Annotations;

namespace P3.Model.Annotations;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Struct |
                AttributeTargets.Enum |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class ExemplaryAttribute : Attribute { }
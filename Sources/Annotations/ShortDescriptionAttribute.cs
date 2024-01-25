using JetBrains.Annotations;

namespace P3Model.Annotations;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Struct |
                AttributeTargets.Enum |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class ShortDescriptionAttribute(string markdownText) : Attribute
{
    public string MarkdownText { get; } = markdownText;
}
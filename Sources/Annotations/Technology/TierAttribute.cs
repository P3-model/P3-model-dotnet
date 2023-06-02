using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly)]
public class TierAttribute : Attribute
{
    public string Name { get; }

    public TierAttribute(string name) => Name = name;
}
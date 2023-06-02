using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly)]
public class DeployableUnitAttribute : Attribute
{
    public string Name { get; }

    public DeployableUnitAttribute(string name) => Name = name;
}
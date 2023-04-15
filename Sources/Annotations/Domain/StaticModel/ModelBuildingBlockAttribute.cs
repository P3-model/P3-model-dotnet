using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Struct |
                AttributeTargets.Enum |
                AttributeTargets.Delegate)]
public class ModelBuildingBlockAttribute : Attribute
{
    public string? Name { get; }

    public ModelBuildingBlockAttribute(string? name = null) => Name = name;
}
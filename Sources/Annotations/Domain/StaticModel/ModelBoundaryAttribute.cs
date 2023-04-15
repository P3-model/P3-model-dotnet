using JetBrains.Annotations;

namespace P3.Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly |
                AttributeTargets.Class |
                AttributeTargets.Struct |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Enum |
                AttributeTargets.Module)]
public class ModelBoundaryAttribute : Attribute
{
    public string? Name { get; }

    public ModelBoundaryAttribute(string? name = null) => Name = name;
}
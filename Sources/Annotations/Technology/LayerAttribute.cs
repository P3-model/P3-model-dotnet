using JetBrains.Annotations;

namespace P3Model.Annotations.Technology;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public class LayerAttribute : Attribute, NamespaceApplicable
{
    public string Name { get; }
    public bool ApplyOnNamespace { get; init; }

    public LayerAttribute(string name) => Name = name;
}
using JetBrains.Annotations;

namespace P3Model.Annotations.People;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public class DevelopmentOwnerAttribute : Attribute, NamespaceApplicable
{
    public string Name { get; }
    public bool ApplyOnNamespace { get; }

    public DevelopmentOwnerAttribute(string name, bool applyOnNamespace = false)
    {
        Name = name;
        ApplyOnNamespace = applyOnNamespace;
    }
}
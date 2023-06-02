using JetBrains.Annotations;

namespace P3Model.Annotations.People;

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public class BusinessOwnerAttribute : Attribute, NamespaceApplicable
{
    public string Name { get; }
    public bool ApplyOnNamespace { get; }

    public BusinessOwnerAttribute(string name, bool applyOnNamespace = false)
    {
        Name = name;
        ApplyOnNamespace = applyOnNamespace;
    }
}
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Struct |
                AttributeTargets.Interface |
                AttributeTargets.Method |
                AttributeTargets.Delegate)]
public class BusinessRuleGuardAttribute : Attribute
{
    public string? Id { get; }
    public string Name { get; }

    public BusinessRuleGuardAttribute(string name)
    {
        Id = null;
        Name = name;
    }
    
    public BusinessRuleGuardAttribute(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
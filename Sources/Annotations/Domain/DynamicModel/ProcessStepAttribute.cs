using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class ProcessStepAttribute : Attribute
{
    public string? Name { get; }
    public string? Process { get; init; }
    public string[] NextSteps { get; init; }

    public ProcessStepAttribute(string? name = null)
    {
        Name = name;
        NextSteps = Array.Empty<string>();
    }

    public ProcessStepAttribute(string name, string process, params string[] nextSteps)
    {
        Name = name;
        Process = process;
        NextSteps = nextSteps;
    }
}
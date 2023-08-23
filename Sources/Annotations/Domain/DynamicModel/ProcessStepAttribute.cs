using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel;

namespace P3Model.Annotations.Domain.DynamicModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Interface |
                AttributeTargets.Delegate |
                AttributeTargets.Method)]
public class ProcessStepAttribute : DomainBuildingBlockAttribute
{
    public string? Process { get; init; }
    public string[] NextSteps { get; init; }

    public ProcessStepAttribute(string? name = null) : base(name)
    {
        NextSteps = Array.Empty<string>();
    }

    public ProcessStepAttribute(string name, string process) : base(name)
    {
        Process = process;
        NextSteps = Array.Empty<string>();
    }
}
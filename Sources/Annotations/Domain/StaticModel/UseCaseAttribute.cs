using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public class UseCaseAttribute : DomainBuildingBlockAttribute
{
    public string? Process { get; init; }

    public UseCaseAttribute(string? name = null) : base(name) { }

    public UseCaseAttribute(string name, string process) : base(name) => Process = process;
}
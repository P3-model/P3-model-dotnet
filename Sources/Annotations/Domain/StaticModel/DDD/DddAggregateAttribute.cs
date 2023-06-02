using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel.DDD;

/// <summary>
/// <para>
/// Indicates that given type is a DDD Aggregate.
/// </para>
/// <para>
/// If you want to learn more check DDD Reference by Eric Evans: https://www.domainlanguage.com/ddd/reference/
/// </para>
/// </summary>
[PublicAPI]
public class DddAggregateAttribute : ModelBuildingBlockAttribute
{
    public DddAggregateAttribute([CallerMemberName] string? name = null) : base(name) { }
}
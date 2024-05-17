using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.DDD;

/// <summary>
/// <para>
/// Indicates that given type is a DDD Aggregate.
/// </para>
/// <para>
/// If you want to learn more check DDD Reference by Eric Evans: https://www.domainlanguage.com/ddd/reference/
/// </para>
/// </summary>
[PublicAPI]
public class DddAggregateAttribute([CallerMemberName] string? name = null) : DomainBuildingBlockAttribute(name);
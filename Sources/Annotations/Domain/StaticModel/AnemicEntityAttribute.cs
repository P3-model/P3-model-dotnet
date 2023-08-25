using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace P3Model.Annotations.Domain.StaticModel;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class |
                AttributeTargets.Struct)]
public class AnemicEntityAttribute : DomainBuildingBlockAttribute
{
    public AnemicEntityAttribute([CallerMemberName] string? name = null) : base(name) { }
}
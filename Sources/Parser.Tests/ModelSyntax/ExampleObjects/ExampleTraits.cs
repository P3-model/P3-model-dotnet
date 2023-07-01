using System.Collections;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective;

namespace Parser.Tests.ModelSyntax.ExampleObjects;

public class ExampleTraits : IEnumerable<Trait>
{
    private readonly Dictionary<Type, Trait> _traits = new();

    public static readonly ExampleTraits All = new()
    {
        new DomainVisionStatement(new Product("ExampleProduct"), null)
    };
    
    private void Add<TTrait>(TTrait trait)
        where TTrait : class, Trait
        => _traits.Add(typeof(TTrait), trait);
    
    public Trait ForType(Type type)
    {
        if (!_traits.TryGetValue(type, out var trait))
            throw new InvalidOperationException($"Missing example for {type.Name}");
        return trait;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Trait> GetEnumerator() => _traits.Values.GetEnumerator();
}
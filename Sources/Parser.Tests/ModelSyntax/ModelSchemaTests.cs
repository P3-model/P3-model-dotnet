using System.Collections.Immutable;
using System.Reflection;
using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting.Json;
using P3Model.Parser.Tests.ModelSyntax.ExampleObjects;

namespace P3Model.Parser.Tests.ModelSyntax;

[TestFixture]
public class ModelSchemaTests
{
    [Test]
    public async Task SerializedModelShouldMatchJsonSchema()
    {
        var system = new DocumentedSystem("TestSystem");
        var assembly = typeof(Model).Assembly;
        var elements = assembly
            .GetTypes()
            .Where(IsAssignableTo<Element>)
            .Where(IsConstructableType)
            .Select(ExampleElements.All.ForType)
            .ToImmutableArray();
        var relations = assembly
            .GetTypes()
            .Where(IsAssignableTo<Relation>)
            .Where(IsConstructableType)
            .Select(ExampleRelations.All.ForType)
            .ToImmutableArray();
        var traits = assembly
            .GetTypes()
            .Where(IsAssignableTo<Trait>)
            .Where(IsConstructableType)
            .Select(ExampleTraits.All.ForType)
            .ToImmutableArray();
        var model = new Model(system, elements, relations, traits);
        
        // TODO: assert json schema
        await using var file = File.Create("model.json");
        await P3ModelSerializer.Serialize(file, model);
    }

    private static bool IsAssignableTo<T>(Type type) =>
        typeof(T).IsAssignableFrom(type);

    private static bool IsConstructableType(Type type) =>
        (type is { IsClass: true, IsAbstract: false } || type.IsValueType) &&
        type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any();
}
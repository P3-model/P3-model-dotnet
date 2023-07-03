using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting.Json.Converters;

namespace P3Model.Parser.OutputFormatting.Json;

public static class P3ModelSerializer
{
    private static readonly JsonConverter[] Converters =
    {
        new PolymorphicJsonConverter<Element>(),
        new PolymorphicJsonConverter<Relation>(),
        new PolymorphicJsonConverter<Trait>(),
        new FileInfoJsonConverter(),
        new HierarchyIdJsonConverter()
    };

    [PublicAPI]
    public static string Serialize(Model model) => JsonSerializer.Serialize(model, CreateOptions());

    [PublicAPI]
    public static async Task Serialize(Stream stream, Model model) =>
        await JsonSerializer.SerializeAsync(stream, model, CreateOptions());

    // PolymorphicJsonConverter calls JsonSerializer.SerializeToDocument method.
    // PreserveReferenceHandler clears reference resolution scope in each call to that method.
    // Thus we need new JsonSerializerOptions object with ReferenceResolver that is not recreated for each JsonSerializer.Serialize method call.
    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = new SerializationScopeReferenceHandler(),
            WriteIndented = true
        };
        foreach (var converter in Converters)
            options.Converters.Add(converter);
        return options;
    }

    private class SerializationScopeReferenceHandler : ReferenceHandler
    {
        private readonly SerializationScopeReferenceResolver _resolver = new();

        public override ReferenceResolver CreateResolver() => _resolver;
    }

    private class SerializationScopeReferenceResolver : ReferenceResolver
    {
        private uint _id;
        private readonly Dictionary<string, object> _idToObjectMap = new();
        private readonly Dictionary<object, string> _objectToIdMap = new();

        public override void AddReference(string referenceId, object value)
        {
            if (!_idToObjectMap.TryAdd(referenceId, value))
                throw new InvalidOperationException();
        }

        public override string GetReference(object value, out bool alreadyExists)
        {
            if (_objectToIdMap.TryGetValue(value, out var id))
            {
                alreadyExists = true;
            }
            else
            {
                _id++;
                id = _id.ToString();
                _objectToIdMap.Add(value, id);
                alreadyExists = false;
            }
            return id;
        }

        public override object ResolveReference(string referenceId)
        {
            if (!_idToObjectMap.TryGetValue(referenceId, out var value))
                throw new InvalidOperationException();
            return value;
        }
    }
}
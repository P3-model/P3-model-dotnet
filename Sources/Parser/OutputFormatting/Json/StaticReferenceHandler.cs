using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace P3Model.Parser.OutputFormatting.Json;

internal class StaticReferenceHandler : ReferenceHandler
{
    private readonly Resolver _resolver = new();

    public override ReferenceResolver CreateResolver() => _resolver;

    private class Resolver : ReferenceResolver
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
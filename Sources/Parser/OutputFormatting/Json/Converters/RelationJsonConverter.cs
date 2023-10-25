using System.Text.Json;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class RelationJsonConverter : ModelJsonConverter<Relation>
{
    protected override void WriteRequiredAttributes(Utf8JsonWriter writer, Relation value)
    {
        writer.WriteString("Type", GetFullTypeName(value));
        writer.WriteString(nameof(Relation.Source), value.Source.Id);
        writer.WriteString(nameof(Relation.Destination), value.Destination.Id);
    }

    protected override bool IsAdditionalAttribute(JsonProperty jsonProperty) => 
        jsonProperty.Name is not (nameof(Relation.Source) or nameof(Relation.Destination));

    private static string GetFullTypeName(Relation relation)
    {
        var type = relation.GetType();
        var fullName = type.Name;
        while (type.DeclaringType != null)
        {
            type = type.DeclaringType;
            fullName = $"{type.Name}.{fullName}";
        }
        return fullName;
    }
}
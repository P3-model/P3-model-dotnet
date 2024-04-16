using System.Text.Json;
using DotNetExtensions;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class RelationJsonConverter : ModelJsonConverter<Relation>
{
    protected override void WriteRequiredAttributes(Utf8JsonWriter writer, Relation value)
    {
        writer.WriteString("Type", value.GetType().GetFullTypeName());
        writer.WriteString(nameof(Relation.Source), value.Source.Id);
        writer.WriteString(nameof(Relation.Destination), value.Destination.Id);
    }

    protected override bool IsAdditionalAttribute(JsonProperty jsonProperty) => 
        jsonProperty.Name is not (nameof(Relation.Source) or nameof(Relation.Destination));
}
using System.Text.Json;
using DotNetExtensions;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class TraitJsonConverter : ModelJsonConverter<Trait>
{
    protected override void WriteRequiredAttributes(Utf8JsonWriter writer, Trait value)
    {
        writer.WriteString("Type", value.GetType().GetFullTypeName());
        writer.WriteString(nameof(Trait.ElementId), value.ElementId.Value);
    }

    protected override bool IsAdditionalAttribute(JsonProperty jsonProperty) => 
        jsonProperty.Name is not (nameof(Trait.ElementId) or nameof(Trait.Element));
}
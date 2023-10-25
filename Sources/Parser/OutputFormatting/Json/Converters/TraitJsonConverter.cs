using System.Text.Json;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class TraitJsonConverter : ModelJsonConverter<Trait>
{
    protected override void WriteRequiredAttributes(Utf8JsonWriter writer, Trait value)
    {
        writer.WriteString("Type", value.GetType().Name);
        writer.WriteString(nameof(Trait.ElementId), value.ElementId);
    }

    protected override bool IsAdditionalAttribute(JsonProperty jsonProperty) => 
        jsonProperty.Name is not (nameof(Trait.ElementId) or nameof(Trait.Element));
}
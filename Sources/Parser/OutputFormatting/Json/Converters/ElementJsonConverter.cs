using System.Text.Json;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class ElementJsonConverter : ModelJsonConverter<Element>
{
    protected override void WriteRequiredAttributes(Utf8JsonWriter writer, Element value)
    {
        writer.WriteString("Type", value.GetType().Name);
        writer.WriteString(nameof(Element.Id), value.Id);
        writer.WriteString(nameof(Element.Name), value.Name);
    }

    protected override bool IsAdditionalAttribute(JsonProperty jsonProperty) => 
        jsonProperty.Name is not (nameof(Element.Perspective) or nameof(Element.Id) or nameof(Element.Name));
}
using System.Text.Json;
using DotNetExtensions;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting.Json.Converters;

internal class ElementJsonConverter : ModelJsonConverter<ElementBase>
{
    protected override void WriteRequiredAttributes(Utf8JsonWriter writer, ElementBase value)
    {
        writer.WriteString("Type", value.GetType().GetFullTypeName());
        writer.WriteString(nameof(ElementBase.Id), value.Id);
        writer.WriteString(nameof(ElementBase.Name), value.Name);
    }

    protected override bool IsAdditionalAttribute(JsonProperty jsonProperty) => 
        jsonProperty.Name is not (nameof(ElementBase.Perspective) or nameof(ElementBase.Id) or nameof(ElementBase.Name));
}
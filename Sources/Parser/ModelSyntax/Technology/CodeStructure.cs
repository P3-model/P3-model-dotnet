namespace P3Model.Parser.ModelSyntax.Technology;

public record CodeStructure(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;

    public record BelongsToLayer(CodeStructure Source, Layer Destination) : Relation<CodeStructure, Layer>;
}
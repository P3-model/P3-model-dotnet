namespace P3Model.Parser.ModelSyntax.Technology;

// TODO: support for code structures placed in several files
public record CodeStructure(string Name, string Path) : Element
{
    public Perspective Perspective => Perspective.Technology;

    public record BelongsToLayer(CodeStructure Source, Layer Destination) : Relation<CodeStructure, Layer>;
}
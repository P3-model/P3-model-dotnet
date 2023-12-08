namespace P3Model.Parser.ModelSyntax.Technology;

// TODO: support for code structures placed in several files
public interface CodeStructure : Element
{
    string Path { get; }
    
    public record BelongsToLayer(CodeStructure Source, Layer Destination) : Relation<CodeStructure, Layer>;
}
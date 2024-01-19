namespace P3Model.Parser.ModelSyntax.Technology;

// TODO: support for code structures placed in several files
public interface CodeStructure : Element
{
    string Path { get; }
    
    public class BelongsToLayer(CodeStructure source, Layer destination) 
        : RelationBase<CodeStructure, Layer>(source, destination);
}
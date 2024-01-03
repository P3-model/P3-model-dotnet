using Humanizer;

namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpNamespace(HierarchyId id, string path)
    : ElementBase(id.Full, id.LastPart.Humanize(LetterCasing.Title)), HierarchyElement, CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public new HierarchyId Id { get; } = id;
    public string Path { get; } = path;

    public record ContainsNamespace(CSharpNamespace Source, CSharpNamespace Destination)
        : HierarchyRelation<CSharpNamespace>;

    public record ContainsType(CSharpNamespace Source, CSharpType Destination) : Relation<CSharpNamespace, CSharpType>;
}
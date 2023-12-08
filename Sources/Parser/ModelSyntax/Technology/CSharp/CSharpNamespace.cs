using Humanizer;

namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpNamespace : ElementBase, HierarchyElement, CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public new HierarchyId Id { get; }
    public string Path { get; }

    public CSharpNamespace(HierarchyId id, string path) : base(id.Full, id.LastPart.Humanize())
    {
        Id = id;
        Path = path;
    }

    public record ContainsNamespace(CSharpNamespace Source, CSharpNamespace Destination)
        : HierarchyRelation<CSharpNamespace>;

    public record ContainsType(CSharpNamespace Source, CSharpType Destination) : Relation<CSharpNamespace, CSharpType>;
}
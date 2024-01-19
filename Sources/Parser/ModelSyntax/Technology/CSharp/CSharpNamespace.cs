using Humanizer;

namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpNamespace(HierarchyId id, string path)
    : ElementBase(id.Full, id.LastPart.Humanize(LetterCasing.Title)), HierarchyElement, CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public new HierarchyId Id { get; } = id;
    public string Path { get; } = path;

    public class ContainsNamespace(CSharpNamespace source, CSharpNamespace destination)
        : HierarchyRelation<CSharpNamespace>(source, destination);

    public class ContainsType(CSharpNamespace source, CSharpType destination) 
        : RelationBase<CSharpNamespace, CSharpType>(source, destination);
}
using Humanizer;

namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpNamespace(ElementId id, HierarchyPath hierarchyPath, string sourceCodePath) :
    ElementBase(id, hierarchyPath.LastPart.Humanize(LetterCasing.Title)),
    HierarchyElement,
    CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;
    public HierarchyPath HierarchyPath { get; } = hierarchyPath;
    public string SourceCodeSourceCodePath { get; } = sourceCodePath;

    public override bool DataEquals(Element? other) =>
        base.DataEquals(other) &&
        other is CSharpNamespace otherCSharpNamespace &&
        HierarchyPath.Equals(otherCSharpNamespace.HierarchyPath) &&
        SourceCodeSourceCodePath == otherCSharpNamespace.SourceCodeSourceCodePath;

    public override string ToString() => $"{base.ToString()} | HierarchyPath: {HierarchyPath}";

    public class ContainsNamespace(CSharpNamespace source, CSharpNamespace destination)
        : HierarchyRelation<CSharpNamespace>(source, destination);

    public class ContainsType(CSharpNamespace source, CSharpType destination)
        : RelationBase<CSharpNamespace, CSharpType>(source, destination);
}
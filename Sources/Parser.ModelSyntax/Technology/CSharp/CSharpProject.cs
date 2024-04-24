namespace P3Model.Parser.ModelSyntax.Technology.CSharp;

public class CSharpProject(ElementId id, string name, string sourceCodePath) : ElementBase(id, name), CodeStructure
{
    public override Perspective Perspective => Perspective.Technology;

    public string SourceCodeSourceCodePath { get; } = sourceCodePath;

    public override bool DataEquals(Element? other) =>
        base.DataEquals(other) &&
        other is CSharpProject otherCSharpProject &&
        SourceCodeSourceCodePath == otherCSharpProject.SourceCodeSourceCodePath;

    public class ReferencesProject(CSharpProject source, CSharpProject destination)
        : HierarchyRelation<CSharpProject>(source, destination);

    public class ContainsNamespace(CSharpProject source, CSharpNamespace destination)
        : RelationBase<CSharpProject, CSharpNamespace>(source, destination);
}
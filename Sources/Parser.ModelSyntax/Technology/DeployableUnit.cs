using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.ModelSyntax.Technology;

public class DeployableUnit(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.Technology;

    public class ContainsCSharpProject(DeployableUnit source, CSharpProject destination) 
        : RelationBase<DeployableUnit, CSharpProject>(source, destination);

    public class UsesDatabase(DeployableUnit source, Database destination) 
        : RelationBase<DeployableUnit, Database>(source, destination);
}
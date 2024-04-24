namespace P3Model.Parser.ModelSyntax.Technology;

public class Tier(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.Technology;

    public class ContainsDeployableUnit(Tier source, DeployableUnit destination) 
        : RelationBase<Tier, DeployableUnit>(source, destination);
}
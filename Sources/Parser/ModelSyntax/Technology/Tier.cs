namespace P3Model.Parser.ModelSyntax.Technology;

public class Tier(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Technology;

    public class ContainsDeployableUnit(Tier source, DeployableUnit destination) 
        : RelationBase<Tier, DeployableUnit>(source, destination);
}
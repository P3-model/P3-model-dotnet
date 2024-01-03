namespace P3Model.Parser.ModelSyntax.Technology;

public class Tier(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Technology;

    public record ContainsDeployableUnit(Tier Source, DeployableUnit Destination) 
        : Relation<Tier, DeployableUnit>;
}
namespace P3Model.Parser.ModelSyntax.Technology;

public class Tier : ElementBase
{
    public override Perspective Perspective => Perspective.Technology;

    public Tier(string name) : base(name) { }
    public Tier(string id, string name) : base(id, name) { }

    public record ContainsDeployableUnit(Tier Source, DeployableUnit Destination) 
        : Relation<Tier, DeployableUnit>;
}
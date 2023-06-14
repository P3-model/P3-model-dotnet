namespace P3Model.Parser.ModelSyntax.Technology;

public record Tier(string Name) : Element
{
    public record ContainsDeployableUnit(Tier Tier, DeployableUnit DeployableUnit) : Relation;
}
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.ModelSyntax.Technology;

public class DeployableUnit(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Technology;

    public record ContainsCSharpProject(DeployableUnit Source, CSharpProject Destination) 
        : Relation<DeployableUnit, CSharpProject>;

    public record UsesDatabase(DeployableUnit Source, Database Destination) : Relation<DeployableUnit, Database>;
}
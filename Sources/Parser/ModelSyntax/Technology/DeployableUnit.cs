using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.ModelSyntax.Technology;

public class DeployableUnit : ElementBase
{
    public override Perspective Perspective => Perspective.Technology;

    public DeployableUnit(string name) : base(name) { }
    public DeployableUnit(string id, string name) : base(id, name) { }

    public record ContainsCSharpProject(DeployableUnit Source, CSharpProject Destination) 
        : Relation<DeployableUnit, CSharpProject>;

    public record UsesDatabase(DeployableUnit Source, Database Destination) : Relation<DeployableUnit, Database>;
}
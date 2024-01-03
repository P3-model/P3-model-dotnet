namespace P3Model.Parser.ModelSyntax.Technology;

public class Database(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Technology;

    public record BelongsToCluster(Database Source, DatabaseCluster Destination) : Relation<Database, DatabaseCluster>;
}
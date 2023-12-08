namespace P3Model.Parser.ModelSyntax.Technology;

public class Database : ElementBase
{
    public override Perspective Perspective => Perspective.Technology;

    public Database(string name) : base(name) { }
    public Database(string id, string name) : base(id, name) { }

    public record BelongsToCluster(Database Source, DatabaseCluster Destination) : Relation<Database, DatabaseCluster>;
}
namespace P3Model.Parser.ModelSyntax.Technology;

public record Database(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;
    public string Id => Name;

    public record BelongsToCluster(Database Source, DatabaseCluster Destination) : Relation<Database, DatabaseCluster>;
}
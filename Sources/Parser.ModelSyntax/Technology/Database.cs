namespace P3Model.Parser.ModelSyntax.Technology;

public class Database(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.Technology;

    public class BelongsToCluster(Database source, DatabaseCluster destination) 
        : RelationBase<Database, DatabaseCluster>(source, destination);
}
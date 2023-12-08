namespace P3Model.Parser.ModelSyntax.Technology;

public class DatabaseCluster : ElementBase
{
    public override Perspective Perspective => Perspective.Technology;

    public DatabaseCluster(string name) : base(name) { }
    public DatabaseCluster(string id, string name) : base(id, name) { }
}
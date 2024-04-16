namespace P3Model.Parser.ModelSyntax.Technology;

public class DatabaseCluster(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Technology;
}
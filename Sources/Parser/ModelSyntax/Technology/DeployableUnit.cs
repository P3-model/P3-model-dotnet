using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.ModelSyntax.Technology;

public record DeployableUnit(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;
    public string Id => Name;
    
    public record ContainsCSharpProject(DeployableUnit Source, CSharpProject Destination) 
        : Relation<DeployableUnit, CSharpProject>;

    public record UsesDatabase(DeployableUnit Source, Database Destination) : Relation<DeployableUnit, Database>;
}
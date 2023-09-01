using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.ModelSyntax.Technology;

public record DeployableUnit(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;
    
    public record ContainsCSharpProject(DeployableUnit Source, CSharpProject Destination) 
        : Relation<DeployableUnit, CSharpProject>;
}
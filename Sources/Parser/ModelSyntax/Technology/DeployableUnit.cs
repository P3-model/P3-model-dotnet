namespace P3Model.Parser.ModelSyntax.Technology;

public record DeployableUnit(string Name) : Element
{
    public Perspective Perspective => Perspective.Technology;
    
    public record ContainsCodeStructure(DeployableUnit Source, CodeStructure Destination) 
        : Relation<DeployableUnit, CodeStructure>;
}
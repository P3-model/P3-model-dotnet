using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record ModelBoundary(string Name) : Element
{
    public Perspective Perspective => Perspective.Domain;
    
    public record ContainsDomainModule(ModelBoundary Source, DomainModule Destination) 
        : Relation<ModelBoundary, DomainModule>;

    public record ContainsBuildingBlock(ModelBoundary Source, DomainBuildingBlock Destination) 
        : Relation<ModelBoundary, DomainBuildingBlock>;
    
    public record ContainsProcessStep(ModelBoundary Source, ProcessStep Destination) 
        : Relation<ModelBoundary, ProcessStep>;

    public record IsDeployedInDeployableUnit(ModelBoundary Source, DeployableUnit Destination) 
        : Relation<ModelBoundary, DeployableUnit>;
}
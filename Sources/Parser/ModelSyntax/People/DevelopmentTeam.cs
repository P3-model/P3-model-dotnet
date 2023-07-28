using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public record DevelopmentTeam(string Name) : Element
{
    public Perspective Perspective => Perspective.People;
    
    public record OwnsModelBoundary(DevelopmentTeam Source, ModelBoundary Destination) 
        : Relation<DevelopmentTeam, ModelBoundary>;
    
    public record OwnsDomainModule(DevelopmentTeam Source, DomainModule Destination) 
        : Relation<DevelopmentTeam, DomainModule>;
}
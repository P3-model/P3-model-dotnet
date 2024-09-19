using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.ModelSyntax.People;

public class DevelopmentTeam(ElementId id, string name) : ElementBase(id,  name)
{
    public override Perspective Perspective => Perspective.People;

    public class OwnsModelBoundary(DevelopmentTeam source, ModelBoundary destination) 
        : RelationBase<DevelopmentTeam, ModelBoundary>(source, destination);
    
    public class OwnsDomainModule(DevelopmentTeam source, DomainModule destination) 
        : RelationBase<DevelopmentTeam, DomainModule>(source, destination);
}
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class DevelopmentTeam(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.People;

    public class OwnsDomainModule(DevelopmentTeam source, DomainModule destination) 
        : RelationBase<DevelopmentTeam, DomainModule>(source, destination);
}
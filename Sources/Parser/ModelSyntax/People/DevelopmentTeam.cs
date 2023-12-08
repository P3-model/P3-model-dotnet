using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class DevelopmentTeam : ElementBase
{
    public override Perspective Perspective => Perspective.People;

    public DevelopmentTeam(string name) : base(name) { }
    public DevelopmentTeam(string id, string name) : base(id, name) { }

    public record OwnsDomainModule(DevelopmentTeam Source, DomainModule Destination) 
        : Relation<DevelopmentTeam, DomainModule>;
}
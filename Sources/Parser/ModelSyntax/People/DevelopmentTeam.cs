using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.People;

public record DevelopmentTeam(string Name) : Element
{
    public record OwnsDomainModule(DevelopmentTeam Team, DomainModule DomainModule) : Relation;
    public record OwnsDeployableUnit(DevelopmentTeam Team, DeployableUnit DeployableUnit) : Relation;
}
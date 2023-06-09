using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public record BusinessOrganizationalUnit(string Name) : Element
{
    public record OwnsDomainModule(BusinessOrganizationalUnit Unit, DomainModule DomainModule) : Relation;
}
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class BusinessOrganizationalUnit : ElementBase
{
    public override Perspective Perspective => Perspective.People;

    public BusinessOrganizationalUnit(string name) : base(name) { }
    public BusinessOrganizationalUnit(string id, string name) : base(id, name) { }

    public record OwnsDomainModule(BusinessOrganizationalUnit Source, DomainModule Destination) 
        : Relation<BusinessOrganizationalUnit, DomainModule>;
}
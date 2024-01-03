using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class BusinessOrganizationalUnit(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.People;

    public record OwnsDomainModule(BusinessOrganizationalUnit Source, DomainModule Destination) 
        : Relation<BusinessOrganizationalUnit, DomainModule>;
}
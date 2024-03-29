using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public class BusinessOrganizationalUnit(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.People;

    public class OwnsDomainModule(BusinessOrganizationalUnit source, DomainModule destination) 
        : RelationBase<BusinessOrganizationalUnit, DomainModule>(source, destination);
}
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.ModelSyntax.People;

public class BusinessOrganizationalUnit(ElementId id, string name) : ElementBase(id, name)
{
    public override Perspective Perspective => Perspective.People;

    public class OwnsDomainModule(BusinessOrganizationalUnit source, DomainModule destination) :
        RelationBase<BusinessOrganizationalUnit, DomainModule>(source, destination);
}
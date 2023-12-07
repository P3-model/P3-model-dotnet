using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.People;

public record BusinessOrganizationalUnit(string Name) : Element
{
    public Perspective Perspective => Perspective.People;
    public string Id => Name;
    
    public record OwnsDomainModule(BusinessOrganizationalUnit Source, DomainModule Destination) 
        : Relation<BusinessOrganizationalUnit, DomainModule>;
}